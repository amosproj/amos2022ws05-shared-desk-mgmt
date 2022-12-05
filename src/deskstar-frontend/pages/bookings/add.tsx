import Head from "next/head";

import { IRoom } from "../../types/room";

//TODO: delete this - just used for mockup data
import { GetServerSideProps } from "next";
import { rooms } from "../../rooms";
import { deskTypes } from "../../deskTypes";
import { IDeskType } from "../../types/desktypes";
import { useSession } from "next-auth/react";
import DropDownFilter, {
  stringToSelectable,
} from "../../components/DropDownFilter";

const Bookings = ({
  results,
  types,
}: {
  results: IRoom[];
  types: IDeskType[];
}) => {
  let { data: session } = useSession();

  let buildings: string[] = [];
  let locations: string[] = [];
  let rooms: string[] = [];
  let deskTypes: string[] = [];

  let startDateTime: string;
  let endDateTime: string;

  for (const result of results) {
    buildings.push(result.building);
    locations.push(result.location);
    rooms.push(result.roomName);
  }

  for (const type of types) {
    deskTypes.push(type.typeName);
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
        options={stringToSelectable(locations)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected Locations", selectedOptions);
        }}
      />

      <DropDownFilter
        title="Buildings"
        options={stringToSelectable(buildings)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected Buildings", selectedOptions);
        }}
      />

      <DropDownFilter
        title="Rooms"
        options={stringToSelectable(rooms)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected Rooms", selectedOptions);
        }}
      />

      <DropDownFilter
        title="Types"
        options={stringToSelectable(deskTypes)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected Types", selectedOptions);
        }}
      />

      <br />
      <button type="button" className="btn btn-secondary" onClick={onClick}>
        Search for Desks
      </button>
    </div>
  );
};

//TODO: delete this - this is just for developing this component
export const getServerSideProps: GetServerSideProps = async () => {
  return {
    props: {
      results: rooms,
      types: deskTypes,
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
