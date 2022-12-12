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
import { createBooking } from "../../lib/api/BookingService";
import { IBuilding } from "../../types/building";
import { ILocation } from "../../types/location";
import { IFloor } from "../../types/floor";
import { IRoom } from "../../types/room";
import { IDeskType } from "../../types/desktypes";
import { useState } from "react";
import DeskSearchResults from "../../components/DeskSearchResults";
import { IDesk } from "../../types/desk";

const Bookings = ({ buildings: origBuildings }: { buildings: IBuilding[] }) => {
  let { data: session } = useSession();

  const locations: ILocation[] = origBuildings.map((building) => ({
    locationName: building.location,
  }));

  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>([]);

  const [selectedDeskTypes, setSelectedDeskTypes] = useState<IDeskType[]>([]);
  const [desks, setDesks] = useState<IDesk[]>([]);

  let today = new Date();
  let nextBuisinessDay = getNextWork(today);

  const [startDateTime, setStartDateTime] = useState<Date>(
    new Date(nextBuisinessDay.setHours(8, 0, 0, 0))
  );
  const [endDateTime, setEndDateTime] = useState<Date>(
    getEndDate(nextBuisinessDay)
  );

  async function onBook(
    event: {
      target: Element;
    },
    desk: IDesk,
    setButtonText: Function
  ) {
    if (
      event == null ||
      event.target == null ||
      desk == null ||
      session == null
    )
      return;
    event.target.setAttribute("class", "btn loading");
    let message;

    await createBooking(
      session,
      desk.deskId,
      new Date(startDateTime),
      new Date(endDateTime)
    )
      .then((response) => {
        if (response == "success") {
          message =
            "You successfully booked the desk " +
            desk.deskName +
            " from " +
            startDateTime +
            " to " +
            endDateTime;
          event.target.setAttribute("class", "btn btn-disabled");
          setButtonText("Booked");
        } else {
          console.log(response);
          message = response;
          event.target.setAttribute("class", "btn btn-success");
        }
      })
      .catch((error) => {
        console.error("Error calling createBooking:", error);
        event.target.setAttribute("class", "btn btn-success");
      });
    alert(message);
  }

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
          defaultValue={getFormatedDate(startDateTime)}
          min={getFormatedDate(today)}
          onChange={(event) => setStartDateTime(getUTCDate(event.target.value))}
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
          min={getFormatedDate(new Date(today.setHours(today.getHours() + 1)))}
          defaultValue={getFormatedDate(endDateTime)}
          onChange={(event) => setEndDateTime(getUTCDate(event.target.value))}
        />
      </div>

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

      {desks.length > 0 && (
        <DeskSearchResults
          results={desks.filter(function (e) {
            return selectedDeskTypes
              .map((deskTypes) => deskTypes.typeName)
              .includes(e.deskTyp);
          })}
          onBook={onBook}
        />
      )}
    </div>
  );
};

function getEndDate(tomorrow: Date) {
  let date = new Date(tomorrow);
  date.setHours(17, 0, 0, 0);
  return date;
}

function getFormatedDate(date: Date) {
  const offset = date.getTimezoneOffset();
  return new Date(date.getTime() - offset * 60 * 1000)
    .toISOString()
    .substring(0, "YYYY-MM-DDTHH:MM".length);
}

function getUTCDate(dateString: string) {
  const date = new Date(dateString);
  const offset = date.getTimezoneOffset();
  return new Date(date.getTime() + offset * 60 * 1000);
}

function getNextWork(date: Date) {
  var returnDate = new Date(date);
  returnDate.setDate(returnDate.getDate() + 1);
  if (returnDate.getDay() == 0) returnDate.setDate(returnDate.getDate() + 1);
  else if (returnDate.getDay() == 6)
    returnDate.setDate(returnDate.getDate() + 2);
  return returnDate;
}

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

export default Bookings;
