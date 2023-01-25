import { useSession } from "next-auth/react";
import { useState, Fragment } from "react";
import { toast } from "react-toastify";
import { getDesks, getFloors, getRooms } from "../lib/api/ResourceService";
import { IBuilding } from "../types/building";
import { IDesk } from "../types/desk";
import { IDeskType } from "../types/desktypes";
import { IFloor } from "../types/floor";
import { ILocation } from "../types/location";
import { IRoom } from "../types/room";
import FilterListbox from "./FilterListbox";

type FilterbarProps = {
  buildings: IBuilding[];
  startDateTime: Date;
  endDateTime: Date;
  desks: IDesk[];
  setDesks: (desks: IDesk[]) => void;
  setFilteredDesks: (desks: IDesk[]) => void;
};

export default function Filterbar({
  buildings: origBuildings,
  startDateTime,
  endDateTime,
  desks,
  setDesks,
  setFilteredDesks,
}: FilterbarProps) {
  const { data: session } = useSession();
  let initBuildings = origBuildings.filter(
    (building) => !building.isMarkedForDeletion
  );
  const locations: ILocation[] = initBuildings.map((building) => ({
    locationName: building.location,
  }));
  const [selectedLocation, _setSelectedLocation] = useState<ILocation | null>(
    null
  );

  const [selectedBuilding, _setSelectedBuilding] = useState<IBuilding | null>(
    null
  );
  const [selectedFloor, _setSelectedFloor] = useState<IFloor | null>(null);
  const [selectedRoom, _setSelectedRoom] = useState<IRoom | null>(null);
  const [selectedDeskType, _setSelectedDeskType] = useState<IDeskType | null>(
    null
  );

  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>([]);

  async function setSelectedLocation(selectedLocation: ILocation | null) {
    if (!selectedLocation) {
      setBuildings([]);
      setSelectedBuilding(null);
      return;
    }
    _setSelectedLocation(selectedLocation);

    let buildings = initBuildings.filter(
      (building) => selectedLocation.locationName === building.location
    );

    setBuildings(buildings);
    setSelectedBuilding(null);
  }

  async function setSelectedBuilding(selectedBuilding: IBuilding | null) {
    _setSelectedBuilding(selectedBuilding);
    if (!selectedBuilding) {
      setFloors([]);
      setSelectedFloor(null);
      return;
    }

    if (!session) {
      setFloors([]);
      return;
    }

    try {
      let floors = await getFloors(session, selectedBuilding.buildingId);
      floors = floors.filter((floor) => !floor.isMarkedForDeletion);
      setFloors(floors);
    } catch (error) {
      toast.error(`${error}`);
    }
    setSelectedFloor(null);
  }

  async function setSelectedFloor(selectedFloor: IFloor | null) {
    _setSelectedFloor(selectedFloor);
    if (!selectedFloor) {
      setRooms([]);
      setSelectedRoom(null);
      return;
    }

    if (!session) {
      setRooms([]);
      return;
    }

    try {
      let rooms = await getRooms(session, selectedFloor.floorId);
      rooms = rooms.filter((room) => !room.isMarkedForDeletion);
      setRooms(rooms);
    } catch (error) {
      toast.error(`${error}`);
    }

    setSelectedRoom(null);
  }

  async function setSelectedRoom(selectedRoom: IRoom | null) {
    _setSelectedRoom(selectedRoom);
    if (!selectedRoom) return;

    if (!session) return setRooms([]);

    try {
      const desks = await getDesks(
        session,
        selectedRoom.roomId,
        startDateTime.getTime(),
        endDateTime.getTime()
      );

      const filteredDesks = desks.filter(
        (desk) => desk.bookings.length === 0 && !desk.isMarkedForDeletion
      );

      setDeskTypes(deskTypes);
      setSelectedDeskType(null); // Equals all there

      setDesks(filteredDesks);
      setFilteredDesks(filteredDesks);
    } catch (error) {
      toast.error(`${error}`);
    }
  }

  function setSelectedDeskType(selectedDeskType: IDeskType | null) {
    _setSelectedDeskType(selectedDeskType);
    if (!selectedDeskType) {
      setFilteredDesks(desks);
      return;
    }

    let filteredDesks = desks.filter(
      (desk) => desk.deskTyp === selectedDeskType.deskTypeName
    );
    filteredDesks = filteredDesks.filter(
      (deskType) => !deskType.isMarkedForDeletion
    );
    setFilteredDesks(filteredDesks);
  }

  return (
    <div>
      <div className="flex gap-2 my-4">
        <FilterListbox
          items={locations}
          selectedItem={selectedLocation}
          setSelectedItem={setSelectedLocation}
          getName={(location) =>
            location ? location.locationName : "Select location"
          }
        />

        {buildings.length > 0 && (
          <FilterListbox
            items={buildings}
            selectedItem={selectedBuilding}
            setSelectedItem={setSelectedBuilding}
            getName={(building) => building?.buildingName ?? "Select building"}
            getKey={(building) => building?.buildingId}
          />
        )}

        {floors.length > 0 && (
          <FilterListbox
            items={floors}
            selectedItem={selectedFloor}
            setSelectedItem={setSelectedFloor}
            getName={(floor) => floor?.floorName ?? "Select floor"}
            getKey={(floor) => floor?.floorId}
          />
        )}

        {rooms.length > 0 && (
          <FilterListbox
            items={rooms}
            selectedItem={selectedRoom}
            setSelectedItem={setSelectedRoom}
            getName={(room) => room?.roomName ?? "Select room"}
            getKey={(room) => room?.roomId}
          />
        )}

        {deskTypes.length > 0 && (
          <FilterListbox
            items={deskTypes}
            selectedItem={selectedDeskType}
            setSelectedItem={setSelectedDeskType}
            getName={(deskType) => deskType?.deskTypeName ?? "Select desktype"}
            getKey={(deskType) => deskType.deskTypeId}
            allOption={true}
          />
        )}
      </div>
    </div>
  );
}
