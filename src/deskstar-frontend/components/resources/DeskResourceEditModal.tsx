import { Session } from "next-auth";
import React, { useState } from "react";
import { toast } from "react-toastify";
import { IDesk } from "../../types/desk";
import { IRoom } from "../../types/room";
import Input from "../forms/Input";
import { getFloors, getRooms } from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { ILocation } from "../../types/location";
import FilterListbox from "../FilterListbox";

const DeskResourceEditModal = ({
    desk,
    isOpen,
    setState,
    session,
    updateDesk,
    origBuildings,
    origDeskTypes,
    deskName,
    setDeskName,
}: {
    desk: IDesk | undefined;
    rooms: IRoom[];
    isOpen: boolean;
    setState: Function;
    session: Session;
    origBuildings: IBuilding[];
    origDeskTypes: IDeskType[];
    updateDesk: Function;
    deskName: string;
    setDeskName: Function;
}) => {
    if (!desk) return (<></>)

    const [deskType, setDeskType] = useState<IDeskType | null>();
    const [room, setRoom] = useState<IRoom | null>();
    const [floor, setFloor] = useState<IFloor | null>();
    const [building, setBuilding] = useState<IBuilding | null>();
    const [location, setLocation] = useState<ILocation | null>();
    const uniqueLocation = (ogBuildings: IBuilding[]) => {
        const t = new Map<string, ILocation>();
        ogBuildings.forEach((element) =>
            t.set(element.location, { locationName: element.location })
        );
        return Array.from(t.values());
    };
    const [locations, setLocations] = useState<ILocation[]>(
        uniqueLocation(origBuildings)
    );

    const [buildings, setBuildings] = useState<IBuilding[]>([]);
    const [floors, setFloors] = useState<IFloor[]>([]);
    const [rooms, setRooms] = useState<IRoom[]>([]);
    const [deskTypes, setDeskTypes] = useState<IDeskType[]>(origDeskTypes);

    function clearState() {
        setLocation(null);
        setBuilding(null);
        setFloor(null);
        setRoom(null);
    }
    async function onSelectedLocationChange(
        selectedLocation: ILocation | null | undefined
    ) {
        if (!selectedLocation) {
            return;
        }

        setLocation(selectedLocation);
        let filteredBuildings = origBuildings.filter(
            (building) => selectedLocation.locationName === building.location
        );

        setBuildings(filteredBuildings);
        setBuilding(null);
        setFloor(null);
        setRoom(null);
    }

    async function onSelectedBuildingChange(
        selectedBuilding: IBuilding | null | undefined
    ) {
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

    async function onSelectedFloorChange(
        selectedFloor: IFloor | null | undefined
    ) {
        if (!selectedFloor) {
            return;
        }

        setFloor(selectedFloor);
        if (!session) {
            return [];
        }

        const resRooms = await getRooms(session, selectedFloor.floorId);
        setRooms(resRooms);
        setRoom(null);
    }

    async function onSelectedDeskTypeChange(
        onSelectedDeskType: IDeskType | null | undefined
    ) {
        if (!onSelectedDeskType) {
            return;
        }
        setDeskType(onSelectedDeskType);
    }
    return (
        <>
            <div className={isOpen ? "modal modal-open overflow-y-auto" : "modal"}>
                <div className="modal-box overflow-y-visible">
                    <a className="btn btn-sm btn-circle float-right" onClick={() => { clearState(); setState(false); }}>
                        x
                    </a>
                    <h1 className="text-2xl pb-4">Edit Desk Type</h1>
                    <Input
                        name="Change Desk Name"
                        onChange={(e) => {
                            setDeskName(e.target.value);
                            console.log(e.target.value);
                        }}
                        value={deskName}
                        placeholder={desk.deskName}
                    />
                    <>
                        <div>Change Desk Type</div>
                        <FilterListbox
                            key={"deskTypeListBox"}
                            items={deskTypes}
                            selectedItem={deskType}
                            setSelectedItem={(o) => onSelectedDeskTypeChange(o)}
                            getName={(deskType) =>
                                deskType ? deskType.deskTypeName : "No type selected"
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

                    {location && (
                        <>
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
                    )}

                    {building && (
                        <>
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
                    )}

                    {floor && (
                        <>
                            <div>Room</div>
                            <FilterListbox
                                key={"roomListBox"}
                                items={rooms}
                                selectedItem={room}
                                setSelectedItem={(o) => setRoom(o)}
                                getName={(room) => (room ? room.roomName : "select room")}
                            />
                        </>
                    )}
                    <div className="flex justify-end">
                        <button
                            className="btn bg-deskstar-green-dark text-black hover:bg-deskstar-green-light"
                            onClick={() => {
                                //check changes
                                if (deskName === '') {
                                    toast.error("Desk name must not be empty");
                                    return;
                                }
                                const changeDeskName = desk.deskName === deskName ? undefined : deskName;
                                const changeRoomId = !room || desk.roomId === room.roomId ? undefined : room.roomId;
                                const changeDeskTypeId = desk.deskTyp === deskType?.deskTypeName ? undefined : deskType?.deskTypeId;
                                const deskDto = { deskName: changeDeskName, deskTypeId: changeDeskTypeId, roomId: changeRoomId };
                                updateDesk(deskDto, desk);
                                clearState();
                            }}
                        >Confirm</button>
                    </div>
                </div>
            </div>
        </>
    );
};

export default DeskResourceEditModal;
