import dayjs from "dayjs";
import { useSession } from "next-auth/react";
import { useState, Fragment, useMemo } from "react";
import { toast } from "react-toastify";
import { getAggregatedDesks } from "../lib/api/DesksService";
import { getDesks, getFloors, getRooms } from "../lib/api/ResourceService";
import { IBuilding } from "../types/building";
import { IDesk } from "../types/desk";
import { IDeskType } from "../types/desktypes";
import { IFloor } from "../types/floor";
import { ILocation } from "../types/location";
import { IRoom } from "../types/room";
import FilterListbox from "./FilterListbox";

type FilterbarProps = {
  desks: IDesk[];
  setFilteredDesks: (desks: IDesk[]) => void;
};

export default function Filterbar({ desks, setFilteredDesks }: FilterbarProps) {
  const initBuildings: IBuilding[] = desks
    .map((desk) => ({
      buildingName: desk.buildingName,
      buildingId: desk.buildingId,
      location: desk.location,
      isMarkedForDeletion: false,
    }))
    .filter(
      (building, index, self) =>
        index === self.findIndex((b) => b.buildingId === building.buildingId)
    );

  const initFloors: (IFloor & { buildingId: string })[] = desks
    .map((desk) => ({
      floorName: desk.floorName,
      floorId: desk.floorId,
      buildingName: desk.buildingName,
      buildingId: desk.buildingId,
      location: desk.location,
      isMarkedForDeletion: false,
    }))
    .filter(
      (floor, index, self) =>
        index === self.findIndex((f) => f.floorId === floor.floorId)
    );
  const initRooms: (IRoom & { buildingId: string; floorId: string })[] = desks
    .map((desk) => ({
      roomName: desk.roomName,
      roomId: desk.roomId,
      floor: desk.floorName,
      floorId: desk.floorId,
      building: desk.buildingName,
      buildingId: desk.buildingId,
      location: desk.location,
      isMarkedForDeletion: false,
    }))
    .filter(
      (room, index, self) =>
        index === self.findIndex((r) => r.roomId === room.roomId)
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

  const buildings = useMemo<IBuilding[]>(() => {
    return initBuildings.filter(
      (building) => selectedLocation?.locationName === building.location
    );
  }, [initBuildings, selectedLocation]);

  const floors = useMemo<IFloor[]>(() => {
    return initFloors.filter(
      (floor) => selectedBuilding?.buildingId === floor.buildingId
    );
  }, [initFloors, selectedBuilding]);

  const rooms = useMemo<IRoom[]>(() => {
    return initRooms.filter(
      (room) =>
        selectedFloor?.floorId === room.floorId &&
        selectedBuilding?.buildingId === room.buildingId
    );
  }, [initRooms, selectedFloor, selectedBuilding]);

  const deskTypes = desks
    .map((desk) => ({
      deskTypeName: desk.deskTyp,
      deskTypeId: desk.deskTyp,
      isMarkedForDeletion: false,
    }))
    .filter(
      (deskType, index, self) =>
        index === self.findIndex((dT) => dT.deskTypeId === deskType.deskTypeId)
    );

  useMemo(() => {
    let filteredDesks = desks;
    if (selectedLocation) {
      filteredDesks = filteredDesks.filter(
        (desk) => desk.location === selectedLocation.locationName
      );
    }
    if (selectedBuilding) {
      filteredDesks = filteredDesks.filter(
        (desk) => desk.buildingId === selectedBuilding.buildingId
      );
    }
    if (selectedFloor) {
      filteredDesks = filteredDesks.filter(
        (desk) => desk.floorId === selectedFloor.floorId
      );
    }
    if (selectedRoom) {
      filteredDesks = filteredDesks.filter(
        (desk) => desk.roomId === selectedRoom.roomId
      );
    }
    if (selectedDeskType) {
      filteredDesks = filteredDesks.filter(
        (desk) => desk.deskTyp === selectedDeskType.deskTypeId
      );
    }

    setFilteredDesks(filteredDesks);
  }, [
    desks,
    setFilteredDesks,
    selectedLocation,
    selectedBuilding,
    selectedFloor,
    selectedRoom,
    selectedDeskType,
  ]);

  async function setSelectedLocation(selectedLocation: ILocation | null) {
    _setSelectedLocation(selectedLocation);
    if (!selectedLocation) {
      setSelectedBuilding(null);
      return;
    }
    setSelectedBuilding(null);
  }

  async function setSelectedBuilding(selectedBuilding: IBuilding | null) {
    _setSelectedBuilding(selectedBuilding);
    if (!selectedBuilding) {
      setSelectedFloor(null);
      return;
    }
    setSelectedFloor(null);
  }

  async function setSelectedFloor(selectedFloor: IFloor | null) {
    _setSelectedFloor(selectedFloor);
    if (!selectedFloor) {
      setSelectedRoom(null);
      return;
    }
    setSelectedRoom(null);
  }

  async function setSelectedRoom(selectedRoom: IRoom | null) {
    _setSelectedRoom(selectedRoom);
    if (!selectedRoom) return;
    setSelectedDeskType(null);
  }

  function setSelectedDeskType(selectedDeskType: IDeskType | null) {
    _setSelectedDeskType(selectedDeskType);
    if (!selectedDeskType) {
      setFilteredDesks(desks);
      return;
    }
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
