import Head from "next/head";

import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBuildings } from "../../lib/api/ResourceService";
import { createBooking } from "../../lib/api/BookingService";
import { IBuilding } from "../../types/building";
import { useState } from "react";
import DeskSearchResults from "../../components/DeskSearchResults";
import { IDesk } from "../../types/desk";
import Filterbar from "../../components/Filterbar";
import DesksTable from "../../components/DesksTable";
import { toast } from "react-toastify";

export default function AddBooking({
  buildings: origBuildings,
}: {
  buildings: IBuilding[];
}) {
  let { data: session } = useSession();

  const [desks, setDesks] = useState<IDesk[]>([]);
  const [filteredDesks, setFilteredDesks] = useState<IDesk[]>([]);

  let today = new Date();
  today.setHours(8, 0, 0, 0);
  let nextBusinessDay = getNextBusinessDay(today);

  const [startDateTime, setStartDateTime] = useState<Date>(
    new Date(nextBusinessDay.setHours(8, 0, 0, 0))
  );
  const [endDateTime, setEndDateTime] = useState<Date>(
    new Date(nextBusinessDay.setHours(17, 0, 0, 0))
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

    try {
      let message;

      let response = await createBooking(
        session,
        desk.deskId,
        startDateTime,
        endDateTime
      );

      if (response == "success") {
        message = `You successfully booked the desk ${desk.deskName} from ${startDateTime} to ${endDateTime}`;
        event.target.setAttribute("class", "btn btn-disabled");
        setButtonText("Booked");
        toast.success(message);
      } else {
        console.log(response);
        message = response;
        event.target.setAttribute("class", "btn btn-success");
        toast.error(message);
      }
    } catch (error) {
      toast.error(`Error calling createBooking: ${error}`);
      event.target.setAttribute("class", "btn btn-success");
    }
  }

  return (
    <div>
      <Head>
        <title>Add New Booking</title>
      </Head>
      <h1 className="text-4xl mb-5">Book a desk</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        <div className="form-control">
          <label className="label" htmlFor="start-date">
            <b>Start: </b>
          </label>
          <input
            className="input input-bordered"
            type="datetime-local"
            id="start-date-time"
            name="Start"
            defaultValue={formatDateForInputField(startDateTime)}
            min={formatDateForInputField(today)}
            onChange={(event) => setStartDateTime(new Date(event.target.value))}
          />
        </div>

        <div className="form-control">
          <label className="label" htmlFor="end-date">
            <b>End: </b>
          </label>
          <input
            className="input input-bordered"
            type="datetime-local"
            id="end-date-time"
            // Bind the value of the input to enddatetime
            min={formatDateForInputField(
              new Date(startDateTime.setHours(startDateTime.getHours() + 1))
            )}
            defaultValue={formatDateForInputField(endDateTime)}
            onChange={(event) => setEndDateTime(new Date(event.target.value))}
          />
        </div>
      </div>

      <div className="my-4"></div>

      <Filterbar
        buildings={origBuildings}
        desks={desks}
        setDesks={setDesks}
        setFilteredDesks={setFilteredDesks}
        startDateTime={startDateTime}
        endDateTime={endDateTime}
      />

      {filteredDesks.length > 0 && (
        <DesksTable desks={filteredDesks} onBook={onBook} />
        // <DeskSearchResults results={filteredDesks} onBook={onBook} />
      )}
    </div>
  );
}

function formatDateForInputField(date: Date) {
  const offset = date.getTimezoneOffset();

  return new Date(date.getTime() - offset * 60 * 1000)
    .toISOString()
    .substring(0, "YYYY-MM-DDTHH:MM".length);
}

function getNextBusinessDay(date: Date) {
  var returnDate = new Date(date);
  returnDate.setDate(returnDate.getDate() + 1);

  if (returnDate.getDay() == 0) {
    returnDate.setDate(returnDate.getDate() + 1);
  } else if (returnDate.getDay() == 6) {
    returnDate.setDate(returnDate.getDate() + 2);
  }

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
