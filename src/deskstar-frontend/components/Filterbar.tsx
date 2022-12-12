import { useSession } from "next-auth/react";
import { useState } from "react";
import { getDesks, getFloors, getRooms } from "../lib/api/ResourceService";
import { IBuilding } from "../types/building";
import { IDesk } from "../types/desk";
import { IDeskType } from "../types/desktypes";
import { IFloor } from "../types/floor";
import { ILocation } from "../types/location";
import { IRoom } from "../types/room";
import DropDownFilter from "./DropDownFilter";

type FilterbarProps = {
  buildings: IBuilding[];
  startDateTime: Date;
  endDateTime: Date;
  desks: IDesk[];
  setDesks: (desks: IDesk[]) => void;
};

export default function Filterbar({
  buildings: origBuildings,
  startDateTime,
  endDateTime,
  desks,
  setDesks,
}: FilterbarProps) {
  const { data: session } = useSession();

  const locations: ILocation[] = origBuildings.map((building) => ({
    locationName: building.location,
  }));

  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>([]);
  const [selectedDeskTypes, setSelectedDeskTypes] = useState<IDeskType[]>([]);

  async function onSelectedLocationChange(selectedLocations: ILocation[]) {
    let buildings = origBuildings.filter((building) =>
      selectedLocations.some((location) => {
        return location.locationName === building.location;
      })
    );

    setBuildings(buildings);
  }

  async function onSelectedBuildingChange(selectedBuildings: IBuilding[]) {
    const promises = await Promise.all(
      selectedBuildings.map(async (building) => {
        if (!session) {
          return [];
        }

        const resFloors = await getFloors(session, building.buildingId);

        return resFloors;
      })
    );

    setFloors(promises.flat());
  }

  async function onSelectedFloorChange(selectedFloors: IFloor[]) {
    const promises = await Promise.all(
      selectedFloors.map(async (floor) => {
        if (!session) {
          return [];
        }

        const resRooms = await getRooms(session, floor.floorID);
        return resRooms;
      })
    );

    setRooms(promises.flat());
  }

  async function onSelectedRoomChange(selectedRooms: IRoom[]) {
    const promises = await Promise.all(
      selectedRooms.map(async (room) => {
        if (!session) {
          return [];
        }

        const resDeskType = await getDesks(
          session,
          room.roomId,
          startDateTime.getTime(),
          endDateTime.getTime()
        );

        return resDeskType;
      })
    );

    const desks = promises.flat();
    const filteredDesks = desks.filter((desk) => desk.bookings.length === 0);
    console.log(filteredDesks);
    setDesks(filteredDesks);

    let deskTypes = filteredDesks.map((desk) => ({
      typeId: desk.deskTyp,
      typeName: desk.deskTyp,
    }));

    setDeskTypes(deskTypes);
    setSelectedDeskTypes(deskTypes);
  }

  function onSelectedDeskTypeChange(selectedDeskTypes: IDeskType[]) {
    setSelectedDeskTypes(
      selectedDeskTypes.length === 0 ? deskTypes : selectedDeskTypes
    );
  }

  return (
    <div>
      <DropDownFilter
        title="Locations"
        getItemName={(location) => location.locationName}
        options={locations}
        setSelectedOptions={onSelectedLocationChange}
      />

      {buildings.length > 0 && (
        <DropDownFilter
          title="Buildings"
          getItemName={(building) => building.buildingName}
          options={buildings}
          setSelectedOptions={onSelectedBuildingChange}
        />
      )}

      {floors.length > 0 && (
        <DropDownFilter
          title="Floors"
          getItemName={(floor) => floor.floorName}
          options={floors}
          setSelectedOptions={onSelectedFloorChange}
        />
      )}

      {rooms.length > 0 && (
        <DropDownFilter
          title="Rooms"
          getItemName={(room) => room.roomName}
          options={rooms}
          setSelectedOptions={onSelectedRoomChange}
        />
      )}

      {deskTypes.length > 0 && (
        <DropDownFilter
          title="Types"
          getItemName={(deskType) => deskType.typeName}
          options={deskTypes}
          setSelectedOptions={onSelectedDeskTypeChange}
        />
      )}

      <div className="my-4"></div>

      {buildings.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a location</span>
          </div>
        </div>
      )}
      {!(buildings.length == 0) && floors.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a building</span>
          </div>
        </div>
      )}
      {!(floors.length == 0) && rooms.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a floor</span>
          </div>
        </div>
      )}
      {!(rooms.length == 0) && deskTypes.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a room</span>
          </div>
        </div>
      )}
    </div>
  );
}
