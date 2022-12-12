import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import Head from "next/head";
import BookingsTable from "../components/BookingsTable";
import { IBooking } from "../types/booking";
//TODO: delete this when fetching from database
import { bookings } from "../bookings";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "./api/auth/[...nextauth]";
import { getBookings } from "../lib/api/BookingService";
import { useState } from "react";
import { classes } from "../lib/helpers";

export default function AppHome({
  bookingsToday,
  bookingsTomorrow,
}: {
  bookingsToday: IBooking[];
  bookingsTomorrow: IBooking[];
}) {
  const { data: session } = useSession();

  //console.log(session?.accessToken);

  const [buttonText, setButtonText] = useState("Check in");
  const [checkedIn, setCheckedIn] = useState(false);

  const handleCheckIn = (text: string) => {
    setButtonText(text);
    setCheckedIn(!checkedIn);
  };

  return (
    <div>
      <Head>
        <title>Dashboard</title>
      </Head>
      <h1 className="text-3xl font-bold text-center mt-10">
        Hello {session?.user?.name}, welcome back to Deskstar
      </h1>
      <h1 className="text-2xl font-bold text-left my-10">Today</h1>
      <BookingsTable bookings={bookingsToday} />
      {bookingsToday.length === 0 && (
        <h1 className="text-l text-center my-10">You have no bookings today</h1>
      )}
      {bookingsToday.length != 0 && (
        <div className="flex justify-center">
          <button
            className={classes(
              "btn dark:text-black hover:dark:text-white border-none",
              checkedIn ? "bg-red-500" : "bg-green-500"
            )}
            onClick={() =>
              handleCheckIn(
                buttonText === "Check in" ? "Check out" : "Check in"
              )
            }
          >
            {buttonText}
          </button>
        </div>
      )}

      <h1 className="text-2xl font-bold text-left my-10">Upcoming Bookings</h1>
      <BookingsTable bookings={bookingsTomorrow} />
    </div>
  );
}

export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (session) {
    const start = new Date();
    start.setHours(0, 0, 0);

    const bookings = await getBookings(session, {
      n: 10,
      direction: "DESC",
      start: start.getTime(),
    });

    const bookingsToday = bookings.filter((booking: IBooking) => {
      const today = new Date().toISOString();
      const startOfDay = new Date(today);
      startOfDay.setHours(0, 0, 0);
      const endOfDay = new Date(today);
      endOfDay.setHours(23, 59, 59);
      const startTime = new Date(booking.startTime).toISOString();
      const endTime = new Date(booking.endTime).toISOString();

      return (
        (startTime >= startOfDay.toISOString() &&
          startTime <= endOfDay.toISOString()) ||
        (endTime >= startOfDay.toISOString() &&
          endTime <= endOfDay.toISOString())
      );
    });

    const bookingsTomorrow = bookings.filter((booking: IBooking) => {
      const todayTimestamp = new Date().getTime();

      let tomorrowTimestamp = todayTimestamp + 86400000;
      let tomorrow = new Date(tomorrowTimestamp);
      tomorrow.setHours(0, 0, 0);
      const startTime = new Date(booking.startTime).getTime();
      return startTime >= tomorrow.getTime();
    });

    return {
      props: {
        bookingsToday,
        bookingsTomorrow,
      },
    };
  } else {
    return {
      props: {
        bookings: [],
      },
    };
  }
};
