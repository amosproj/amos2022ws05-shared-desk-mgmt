import { GetServerSideProps } from "next";
import { unstable_getServerSession } from "next-auth";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { getDesks, getDeskTypes, getFloors, getRooms } from "../lib/api/ResourceService";
import { authOptions } from "../pages/api/auth/[...nextauth]";
import { IBuilding } from "../types/building";
import { IDesk } from "../types/desk";
import { IFloor } from "../types/floor";
import { ILocation } from "../types/location";
import { IRoom } from "../types/room";
import FilterListbox from "./FilterListbox";
import Input from "./forms/Input";

const AddResourceModal = ({ buildings: origBuildings }: { buildings: IBuilding[] }) => {
    let { data: session } = useSession();

    const resourceTypes: string[] = ["Building", "Floor", "Room", "Desk"];
    const [selectedResourceType, setSelectedResourceType] = useState("Desk");

    const [desk, setDesk] = useState("");
    const [deskType, setDeskType] = useState("");
    const [room, setRoom] = useState<IRoom | null>();
    const [floor, setFloor] = useState<IFloor | null>();
    const [building, setBuilding] = useState<IBuilding | null>();
    const [location, setLocation] = useState<ILocation | null>();

    const locations: ILocation[] = origBuildings.map((building) => ({
        locationName: building.location,
    }));

    const [buildings, setBuildings] = useState<IBuilding[]>([]);
    const [floors, setFloors] = useState<IFloor[]>([]);
    const [rooms, setRooms] = useState<IRoom[]>([]);
    const [desks, setDesks] = useState<IDesk[]>([]);

    async function onSelectedLocationChange(selectedLocation: ILocation | null | undefined) {
        if (!selectedLocation) {
            return;
        }

        setLocation(selectedLocation);
        let buildings = origBuildings.filter((building) =>
            selectedLocation.locationName === building.location
        );

        setBuildings(buildings);
        setBuilding(null);
        setFloor(null);
        setRoom(null);
    }

    async function onSelectedBuildingChange(selectedBuilding: IBuilding | null | undefined) {
        if (!selectedBuilding) {
            return;
        }

        setBuilding(selectedBuilding);
        if (!session) {
            return [];
        }
        const resFloors = await getFloors(session, selectedBuilding.buildingId);
        setFloors(resFloors);

        setFloor(null);
        setRoom(null);
    }

    async function onSelectedFloorChange(selectedFloor: IFloor | null | undefined) {
        if (!selectedFloor) {
            return;
        }

        setFloor(selectedFloor);
        if (!session) {
            return [];
        }
        const resRooms = await getRooms(session, selectedFloor.floorID);
        setRooms(resRooms);

        setRoom(null);
    }

    return <>
        <div id="create-resource-modal" className="modal">
            <div className="modal-box text-left">
                <a href="#close" className="btn btn-sm btn-circle float-right">
                    x
                </a>
                <h1 className="text-2xl pb-4">Ressource Hinzufügen</h1>

                <div>Ressourcentyp</div>

                <div className="flex items-center">
                    {resourceTypes.map((type: string) => (
                        <button
                            className="btn mr-2"
                            onClick={() => setSelectedResourceType(type)}>
                            {type}
                        </button>
                    ))}
                </div>

                {selectedResourceType !== "Desk" && <div className="py-5">Zur Zeit können nur Tische hinzugefügt werden.</div>}

                {selectedResourceType === "Desk" && <>
                    <Input name="Desk" onChange={(e) => { setDesk(e.target.value) }} value={desk} placeholder="Desk Name" />
                    <Input name="Desk Types" onChange={(e) => { setDeskType(e.target.value) }} value={deskType} placeholder="Desk Type" />

                    <div>Ort</div>
                    <FilterListbox
                        items={locations}
                        selectedItem={location}
                        setSelectedItem={(o) => onSelectedLocationChange(o)}
                        getName={(location) =>
                            location ? location.locationName : "Kein Ort ausgewählt"
                        }
                    />

                    {location && <>
                        <div>Gebäude</div>
                        <FilterListbox
                            items={buildings}
                            selectedItem={building}
                            setSelectedItem={(o) => onSelectedBuildingChange(o)}
                            getName={(building) =>
                                building ? building.buildingName : "Kein Gebäude ausgewählt"
                            }
                        />
                    </>}

                    {building && <>
                        <div>Stockwerk</div>
                        <FilterListbox
                            items={floors}
                            selectedItem={floor}
                            setSelectedItem={(o) => onSelectedFloorChange(o)}
                            getName={(floor) =>
                                floor ? floor.floorName : "Kein Stockwerk ausgewählt"
                            }
                        />
                    </>}

                    {floor && <>
                        <div>Raum</div>
                        <FilterListbox
                            items={rooms}
                            selectedItem={room}
                            setSelectedItem={(o) => setRoom(o)}
                            getName={(room) =>
                                room ? room.roomName : "Kein Raum ausgewählt"
                            }
                        />
                    </>}
                </>}

                <a href="#close" className="btn text-black bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light float-right">
                    Hinzufügen
                </a>
            </div>
        </div>
    </>;

}

export default AddResourceModal;
