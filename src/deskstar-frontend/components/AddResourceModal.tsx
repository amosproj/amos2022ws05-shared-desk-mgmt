import { GetServerSideProps } from "next";
import { unstable_getServerSession } from "next-auth";
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { createBuilding, createDesk, createDeskType, createFloor, createRoom, getDesks, getDeskTypes, getFloors, getRooms } from "../lib/api/ResourceService";
import { authOptions } from "../pages/api/auth/[...nextauth]";
import { IBuilding } from "../types/building";
import { IDesk } from "../types/desk";
import { IDeskType } from "../types/desktypes";
import { IFloor } from "../types/floor";
import { ILocation } from "../types/location";
import { IRoom } from "../types/room";
import FilterListbox from "./FilterListbox";
import Input from "./forms/Input";

const AddResourceModal = ({ buildings: origBuildings, deskTypes: origDeskTypes }: { buildings: IBuilding[], deskTypes: IDeskType[] }) => {
    let { data: session } = useSession();


    const resourceTypes: string[] = ["Building", "Floor", "Room", "Desk", "DeskType"];
    const [selectedResourceType, setSelectedResourceType] = useState("Desk");

    const [buildingName, setBuildingName] = useState("");
    const [locationName, setLocationName] = useState("");
    const [floorName, setFloorName] = useState("");
    const [roomName, setRoomName] = useState("");
    const [deskName, setDeskName] = useState("");
    const [deskTypeName, setDeskTypeName] = useState("");
    //TODO: get companyID injected to component
    const companyId = "";
    const [deskType, setDeskType] = useState<IDeskType | null>();
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
    const [deskTypes, setDeskTypes] = useState<IDeskType[]>(origDeskTypes);

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
    async function onSelectedDeskTypeChange(onSelectedDeskType: IDeskType | null | undefined) {
        if (!onSelectedDeskType) {
            return;
        }
        setDeskType(onSelectedDeskType);

    }
    async function addBuilding() {
        if (!session)
            return;
        if (!buildingName) {
            alert("please enter a building name");
            return;
        }
        if (!locationName) {
            alert("please enter a location");
            return;
        }
        alert(await createBuilding(session, { companyId: companyId, buildingName: buildingName, location: locationName }));
    }
    async function addFloor() {
        if (!session)
            return;
        if (!floorName) {
            alert("please enter a floor name");
            return;
        }
        if (!location) {
            alert("please choose a location");
            return;
        }
        if (!building) {
            alert("please choose a building");
            return;
        }
        alert(await createFloor(session, { BuildingId: building.buildingId, floorName: floorName }));
    }
    async function addRoom() {
        if (!session)
            return;
        if (!roomName) {
            alert("please enter a room name");
            return;
        }
        if (!location) {
            alert("please choose a location");
            return;
        }
        if (!building) {
            alert("please choose a building");
            return;
        }
        if (!floor) {
            alert("please choose a floor");
            return;
        }
        alert(await createRoom(session, { floorId: floor.floorID, roomName: roomName }));
    }
    async function addDesk() {
        if (!session)
            return;
        if (deskName == "") {
            alert("please enter a desk name");
            return;
        }
        if (!deskType) {
            alert("please choose a desk type");
            return;
        }
        if (!location) {
            alert("please choose a location");
            return;
        }
        if (!building) {
            alert("please choose a building");
            return;
        }
        if (!floor) {
            alert("please choose a floor");
            return;
        }
        if (!room) {
            alert("please choose a room");
            return;
        }
        alert(await createDesk(session, { deskName: deskName, deskTypeId: deskType.typeId, roomId: room?.roomId }));
    }
    async function addDeskType() {
        if (!session)
            return;
        if (!deskTypeName) {
            alert("please enter a desk type name");
            return;
        }
        alert(await createDeskType(session, { companyId: companyId, deskTypeName: deskTypeName }));
    }

    return <>
        <div id="create-resource-modal" className="modal">
            <div className="modal-box text-left">
                <a href="#close" className="btn btn-sm btn-circle float-right">
                    x
                </a>
                <h1 className="text-2xl pb-4">Add Resources</h1>

                <div>Resource Type</div>

                <div className="flex items-center">
                    {resourceTypes.map((type: string) => (
                        <button
                            key={type}
                            className="btn mr-2"
                            onClick={() => setSelectedResourceType(type)}
                        >
                            {type}
                        </button>
                    ))}
                </div>

                {
                    selectedResourceType === "Building" && <>
                        <Input name="Building" onChange={(e) => { setBuildingName(e.target.value) }} value={deskName} placeholder="Building Name" />
                        <Input name="Location" onChange={(e) => { setLocationName(e.target.value) }} value={deskName} placeholder="Location" />

                        <div>Location</div>
                        <FilterListbox
                            key={"locationListBox"}
                            items={locations}
                            selectedItem={location}
                            setSelectedItem={(o) => onSelectedLocationChange(o)}
                            getName={(location) =>
                                location ? location.locationName : "select location"
                            }
                        />

                        <a className="btn text-black bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light float-right" onClick={() => addBuilding()}>
                            Add
                        </a>
                    </>
                }
                {
                    selectedResourceType === "Floor" && <>
                        <Input name="Floor" onChange={(e) => { setFloorName(e.target.value) }} value={deskName} placeholder="Floor Name" />

                        <div>Location</div>
                        <FilterListbox
                            key={"locationListBox"}
                            items={locations}
                            selectedItem={location}
                            setSelectedItem={(o) => onSelectedLocationChange(o)}
                            getName={(location) =>
                                location ? location.locationName : "select location"
                            }
                        />

                        {location && <>
                            <div>Building</div>
                            <FilterListbox
                                key={"buildingListBox"}
                                items={buildings}
                                selectedItem={building}
                                setSelectedItem={(o) => onSelectedBuildingChange(o)}
                                getName={(building) =>
                                    building ? building.buildingName : "select building"
                                }
                            />
                        </>
                        }
                        <a className="btn text-black bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light float-right" onClick={() => addFloor()}>
                            Add
                        </a>
                    </>
                }
                {
                    selectedResourceType === "Room" && <>
                        <Input name="Room" onChange={(e) => { setRoomName(e.target.value) }} value={deskName} placeholder="Room Name" />

                        <div>Location</div>
                        <FilterListbox
                            key={"locationListBox"}
                            items={locations}
                            selectedItem={location}
                            setSelectedItem={(o) => onSelectedLocationChange(o)}
                            getName={(location) =>
                                location ? location.locationName : "select location"
                            }
                        />

                        {location && <>
                            <div>Building</div>
                            <FilterListbox
                                key={"buildingListBox"}
                                items={buildings}
                                selectedItem={building}
                                setSelectedItem={(o) => onSelectedBuildingChange(o)}
                                getName={(building) =>
                                    building ? building.buildingName : "select building"
                                }
                            />
                        </>}

                        {building && <>
                            <div>Floor</div>
                            <FilterListbox
                                key={"floorListBox"}
                                items={floors}
                                selectedItem={floor}
                                setSelectedItem={(o) => onSelectedFloorChange(o)}
                                getName={(floor) =>
                                    floor ? floor.floorName : "select floor"
                                }
                            />
                        </>
                        }
                        <a className="btn text-black bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light float-right" onClick={() => addRoom()}>
                            Add
                        </a>
                    </>
                }


                {selectedResourceType === "Desk" && <>
                    <Input name="Desk" onChange={(e) => { setDeskName(e.target.value) }} value={deskName} placeholder="Desk Name" />
                    <>
                        <div>Desk Type</div>
                        <FilterListbox
                            key={"deskTypeListBox"}
                            items={deskTypes}
                            selectedItem={deskType}
                            setSelectedItem={(o) => onSelectedDeskTypeChange(o)}
                            getName={(deskType) =>
                                deskType ? deskType.typeName : "No type selected"
                            }
                        />
                    </>

                    <div>Location</div>
                    <FilterListbox
                        key={"locationListBox"}
                        items={locations}
                        selectedItem={location}
                        setSelectedItem={(o) => onSelectedLocationChange(o)}
                        getName={(location) =>
                            location ? location.locationName : "select location"
                        }
                    />

                    {location && <>
                        <div>Building</div>
                        <FilterListbox
                            key={"buildingListBox"}
                            items={buildings}
                            selectedItem={building}
                            setSelectedItem={(o) => onSelectedBuildingChange(o)}
                            getName={(building) =>
                                building ? building.buildingName : "select building"
                            }
                        />
                    </>}

                    {building && <>
                        <div>Floor</div>
                        <FilterListbox
                            key={"floorListBox"}
                            items={floors}
                            selectedItem={floor}
                            setSelectedItem={(o) => onSelectedFloorChange(o)}
                            getName={(floor) =>
                                floor ? floor.floorName : "select floor"
                            }
                        />
                    </>}

                    {floor && <>
                        <div>Room</div>
                        <FilterListbox
                            key={"roomListBox"}
                            items={rooms}
                            selectedItem={room}
                            setSelectedItem={(o) => setRoom(o)}
                            getName={(room) =>
                                room ? room.roomName : "select room"
                            }
                        />
                    </>}
                    <a className="btn text-black bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light float-right" onClick={() => addDesk()}>
                        Add
                    </a>
                </>}

                {
                    selectedResourceType === "DeskType" && <>
                        <Input name="Desk Type" onChange={(e) => { setDeskTypeName(e.target.value) }} value={deskName} placeholder="Desk Type Name" />
                        <a className="btn text-black bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light float-right" onClick={() => addDeskType()}>
                            Add
                        </a>
                    </>
                }
            </div>
        </div>
    </>;

}

export default AddResourceModal;

