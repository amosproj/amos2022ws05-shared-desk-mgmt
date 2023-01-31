import { Session } from "next-auth";
import React, { useState } from "react";
import { toast } from "react-toastify";
import { getFloors } from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { IFloor } from "../../types/floor";
import { ILocation } from "../../types/location";
import { IRoom } from "../../types/room";
import FilterListbox from "../FilterListbox";
import Input from "../forms/Input";

const RoomResourceEditModal = ({
  room,
  isOpen,
  setState,
  origBuildings,
  updateRoom,
  session,
  roomName,
  setRoomName,
}: {
  room: IRoom | undefined;
  isOpen: boolean;
  setState: Function;
  origBuildings: IBuilding[];
  updateRoom: Function;
  session: Session;
  roomName: string;
  setRoomName: Function;
}) => {
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
  const [building, setBuilding] = useState<IBuilding | null>();
  const [location, setLocation] = useState<ILocation | null>();
  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floor, setFloor] = useState<IFloor | null>();
  const [floors, setFloors] = useState<IFloor[]>([]);
  function clearState() {
    setLocation(null);
    setBuilding(null);
    setFloor(null);
  }

  if (!room) return <></>;

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
  }
  return (
    <>
      <div className={isOpen ? "modal modal-open overflow-y-auto" : "modal"}>
        <div className="modal-box overflow-y-visible">
          <a
            className="btn btn-sm btn-circle float-right"
            onClick={() => {
              clearState();
              setState(false);
            }}
          >
            x
          </a>
          <h1 className="text-2xl pb-4">Edit Room</h1>
          <h6 className="text-lg pt-1">Change Room Name</h6>
          <Input
            name=""
            onChange={(e) => {
              setRoomName(e.target.value);
            }}
            value={roomName}
            placeholder="Room Name"
          />

          <h6 className="text-lg pt-1">Select New Floor</h6>
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
                getName={(floor) => (floor ? floor.floorName : "select floor")}
              />
            </>
          )}
          <div className="flex justify-end">
            <button
              className="btn bg-deskstar-green-dark text-black hover:bg-deskstar-green-light"
              onClick={() => {
                //check changes
                if (roomName === "") {
                  toast.error("Room name must not be empty");
                  return;
                }
                const changeRoomName =
                  room.roomName === roomName ? undefined : roomName;
                const selectedFloor =
                  !floor ||
                  (floor.floorName === room.floor &&
                    building?.buildingName == room.building)
                    ? undefined
                    : floor;
                const updateRoomDto = {
                  roomName: changeRoomName,
                  floorId: selectedFloor?.floorId,
                };
                updateRoom(updateRoomDto, room);
                clearState();
              }}
            >
              Confirm
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default RoomResourceEditModal;
