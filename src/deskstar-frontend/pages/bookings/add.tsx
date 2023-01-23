import Head from "next/head";

import dayjs from "dayjs";

import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBuildings } from "../../lib/api/ResourceService";
import { createBooking } from "../../lib/api/BookingService";
import { IBuilding } from "../../types/building";
import { useMemo, useRef, useState } from "react";
import DeskSearchResults from "../../components/DeskSearchResults";
import { IDesk } from "../../types/desk";
import Filterbar from "../../components/Filterbar";
import DesksTable from "../../components/DesksTable";
import { toast } from "react-toastify";
import { classes } from "../../lib/helpers";

export default function AddBooking({
  buildings: origBuildings,
}: {
  buildings: IBuilding[];
}) {
  let { data: session } = useSession();

  const [desks, setDesks] = useState<IDesk[]>([]);
  const [filteredDesks, setFilteredDesks] = useState<IDesk[]>([]);

  const today = dayjs()
    .set("minutes", 0)
    .set("seconds", 0)
    .set("milliseconds", 0);

  const endDateTimeRef = useRef<HTMLInputElement>(null);

  const [startDateTime, setStartDateTime] = useState<Date>(
    today.add(1, "hour").toDate()
  );
  const [endDateTime, setEndDateTime] = useState<Date>(
    today.add(2, "hour").toDate()
  );
  const minimumEndDateTime = useMemo(() => {
    return dayjs(startDateTime).add(1, "hour").toDate();
  }, [startDateTime]);

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

    if (endDateTime < minimumEndDateTime) {
      toast.error(
        "The End Time needs to be at minimum 1 hour after the start time."
      );
      return;
    }

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
        message = `You successfully booked the desk ${
          desk.deskName
        } from ${startDateTime.toLocaleDateString()} ${startDateTime.toLocaleTimeString()} to ${endDateTime.toLocaleDateString()} ${endDateTime.toLocaleTimeString()}`;
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
            min={formatDateForInputField(
              dayjs().set("hours", 0).set("minutes", 0).toDate()
            )}
            onChange={(event) => {
              setStartDateTime(dayjs(event.target.value).toDate());

              let newMinimumEndTime = dayjs(event.target.value)
                .add(1, "hour")
                .toDate();

              if (
                endDateTimeRef.current != null &&
                endDateTime < newMinimumEndTime
              ) {
                const newEndTime = dayjs(event.target.value)
                  .add(1, "hour")
                  .toDate();

                endDateTimeRef.current.value =
                  formatDateForInputField(newEndTime);

                setEndDateTime(newEndTime);
              }
            }}
          />
        </div>

        <div className="form-control">
          <label className="label" htmlFor="end-date">
            <b>End: </b>
          </label>
          <input
            className={classes(
              "input input-bordered",
              dayjs(endDateTime).isBefore(dayjs(minimumEndDateTime))
                ? "border-red-600"
                : ""
            )}
            type="datetime-local"
            id="end-date-time"
            ref={endDateTimeRef}
            min={formatDateForInputField(minimumEndDateTime)}
            defaultValue={formatDateForInputField(endDateTime)}
            onChange={(event) => {
              console.log("endDateTime: " + event.target.value);
              console.log("endDateTime: " + event.target.value);

              setEndDateTime(dayjs(event.target.value).toDate());
            }}
          />
          {dayjs(endDateTime).isBefore(dayjs(minimumEndDateTime)) && (
            <span className="text-red-600 ml-2">
              The End Time needs to be at minimum 1 hour after the start time.
            </span>
          )}
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

      {endDateTime >= minimumEndDateTime && filteredDesks.length > 0 && (
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
