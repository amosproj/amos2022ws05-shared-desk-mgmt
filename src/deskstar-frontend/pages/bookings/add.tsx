import Head from "next/head";

import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import DropDownFilter from "../../components/DropDownFilter";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import {
  getBuildings,
  getDesks,
  getFloors,
  getRooms,
} from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { ILocation } from "../../types/location";
import { IFloor } from "../../types/floor";
import { IRoom } from "../../types/room";
import { IDeskType } from "../../types/desktypes";
import { useState } from "react";

const Bookings = ({ buildings: origBuildings }: { buildings: IBuilding[] }) => {
  let { data: session } = useSession();

  const locations: ILocation[] = origBuildings.map((building) => ({
    locationName: building.location,
  }));

  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>([]);

  let startDateTime: string;
  let endDateTime: string;

  async function onSelectedLocationChange(selectedLocations: ILocation[]) {
    console.log(selectedLocations);
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

        console.log(floor);

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

        const resDeskType = await getDesks(session, room.roomId);

        return resDeskType;
      })
    );

    setDeskTypes(
      promises.flat().map((desk) => ({
        typeId: desk.deskType,
        typeName: `Name ${desk.deskType}`,
      }))
    );
  }

  function onSelectedDeskTypeChange(selectedDeskTypes: IDeskType[]) {}

  return (
    <div>
      <Head>
        <title>Add New Booking</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Add New Booking</h1>
      <h1 className="text-2xl font-bold text-center mt-10">
        Hello {session?.user?.name}, book your personal desk.
      </h1>
      <br />

      <div className="form-group">
        <label className="form-label" htmlFor="start-date">
          <b>Start: </b> &nbsp;
        </label>
        <input
          className="form-input"
          type="datetime-local"
          id="start-date-time"
          name="Start"
          defaultValue={new Date()
            .toISOString()
            .substring(0, "YYYY-MM-DDTHH:SS".length)}
          min={new Date().toISOString().substring(0, "YYYY-MM-DDTHH:SS".length)}
          onChange={(event) => (startDateTime = event.target.value)}
        />
      </div>

      <div className="form-group">
        <label className="form-label" htmlFor="end-date">
          <b>End: </b> &nbsp;
        </label>
        <input
          className="form-input"
          type="datetime-local"
          id="end-date-time"
          min={new Date().toISOString().substring(0, "YYYY-MM-DDTHH:SS".length)}
          defaultValue={getEndDate()}
          onChange={(event) => (endDateTime = event.target.value)}
        />
      </div>

      <DropDownFilter
        title="Locations"
        getItemName={(location) => location.locationName}
        options={locations}
        setSelectedOptions={onSelectedLocationChange}
      />

      <DropDownFilter
        title="Buildings"
        getItemName={(building) => building.buildingName}
        options={buildings}
        setSelectedOptions={onSelectedBuildingChange}
      />

      <DropDownFilter
        title="Floors"
        getItemName={(floor) => floor.floorName}
        options={floors}
        setSelectedOptions={onSelectedFloorChange}
      />

      <DropDownFilter
        title="Rooms"
        getItemName={(room) => room.roomName}
        options={rooms}
        setSelectedOptions={onSelectedRoomChange}
      />

      <DropDownFilter
        title="Types"
        getItemName={(deskType) => deskType.typeName}
        options={deskTypes}
        setSelectedOptions={onSelectedDeskTypeChange}
      />

      <br />
      <button type="button" className="btn btn-secondary" onClick={onClick}>
        Search for Desks
      </button>
    </div>
  );
};

//TODO: delete this - this is just for developing this component
export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (session) {
    const buildings = await getBuildings(session);

    return {
      props: {
        buildings,
      },
    };
  }

  return {
    props: {
      buildings: [],
    },
  };
};

function onClick() {}

function getEndDate() {
  let date = new Date();
  date.setHours(date.getHours() + 1);
  return date.toISOString().substring(0, "YYYY-MM-DDTHH:SS".length);
}

export default Bookings;
