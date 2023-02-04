import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import Head from "next/head";
import BookingsTable from "../components/BookingsTable";
import { IBooking } from "../types/booking";
//TODO: delete this when fetching from database
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "./api/auth/[...nextauth]";
import { getBookings } from "../lib/api/BookingService";
import { useState } from "react";
import { classes } from "../lib/helpers";
import Link from "next/link";

export default function AppHome({
  bookingsToday,
  bookingsTomorrow,
}: {
  bookingsToday: IBooking[];
  bookingsTomorrow: IBooking[];
}) {
  const { data: session } = useSession();

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

      {bookingsToday.length > 0 && (
        <>
          <h1 className="text-2xl font-bold text-left my-10">Today</h1>
          <BookingsTable bookings={bookingsToday} />
        </>
      )}
      {bookingsToday.length === 0 && (
        <div className="alert mt-8">
          <div>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              className="stroke-info flex-shrink-0 w-6 h-6"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              ></path>
            </svg>
            <span>
              You have no bookings today. Just book a table{" "}
              <Link className="underline" href="/bookings/add">
                here
              </Link>
              .
            </span>
          </div>
        </div>
      )}
      {bookingsToday.length != 0 && (
        <div className="flex justify-center">
          <button
            className={classes(
              "btn  border-none",
              checkedIn ? "btn-error" : "btn-primary"
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

      {bookingsTomorrow.length > 0 && (
        <>
          <h1 className="text-2xl font-bold text-left mt-10 mb-6">
            Upcoming Bookings
          </h1>
          <BookingsTable bookings={bookingsTomorrow} />
        </>
      )}
    </div>
  );
}

export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (!session) {
    return {
      redirect: {
        destination: "/login",
        permanent: false,
      },
    };
  }

  const start = new Date();
  start.setHours(0, 0, 0);

  let data;
  try {
    data = await getBookings(session, {
      n: 10,
      direction: "ASC",
      start: start.getTime(),
    });
  } catch (error) {
    console.error(error);
    return {
      redirect: {
        destination: "/500",
        permanent: false,
      },
    };
  }

  const bookingsToday = data.bookings.filter((booking: IBooking) => {
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
      (endTime >= startOfDay.toISOString() && endTime <= endOfDay.toISOString())
    );
  });

  const bookingsTomorrow = data.bookings.filter((booking: IBooking) => {
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
};
