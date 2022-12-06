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
      <h1 className="text-2xl font-bold text-left my-10">
        Your bookings today
      </h1>
      <BookingsTable bookings={bookingsToday} />
      {bookingsToday.length === 0 && (
        <h1 className="text-l text-center my-10">You have no bookings today</h1>
      )}
      {bookingsToday.length != 0 && (
        <div className="flex justify-center">
          <button
            className="btn dark:text-black"
            onClick={() =>
              handleCheckIn(
                buttonText === "Check in" ? "Check out" : "Check in"
              )
            }
            style={{ backgroundColor: checkedIn ? "red" : "green" }}
          >
            {buttonText}
          </button>
        </div>
      )}

      <h1 className="text-2xl font-bold text-left my-10">
        Your upcoming bookings
      </h1>
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
      const today = new Date().getTime();
      let endOfDay = new Date();
      endOfDay.setHours(24, 59, 59);
      const startTime = new Date(booking.startTime).getTime();
      return startTime >= today && startTime <= endOfDay.getTime();
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
