import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../../components/BookingsTable";
import { IBooking } from "../../types/booking";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBookings, deleteBooking } from "../../lib/api/BookingService";
import { useState } from "react";
import { useSession } from "next-auth/react";

export default function Bookings({ bookings }: { bookings: IBooking[] }) {
  const [showAlertSuccess, setShowAlertSuccess] = useState(false);
  const [showAlertError, setShowAlertError] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");

  const { data: session } = useSession();

  async function onDelete(booking: IBooking) {
    console.log(`Pressed delete on ${booking.bookingId}`);

    try {
      let response = await deleteBooking(session, booking.bookingId);

      if (response == "success") {
        setAlertMessage("Booking successfully deleted!");
        setShowAlertSuccess(true);

        let index = bookings.indexOf(booking);
        if (index > -1) {
          bookings.splice(index, 1);
        }
        return;
      } else {
        setAlertMessage(response);
        setShowAlertError(true);
      }
    } catch (error) {
      console.error("Error calling createBooking:", error);
      setAlertMessage("Error calling Server:" + error);
      setShowAlertError(true);
      return;
    }
  }
  const onEdit = (booking: IBooking) => {
    //TODO: implement
    console.log(`Pressed edit on ${booking.bookingId}`);
  };

  function onClose() {
    setShowAlertSuccess(false);
    setShowAlertError(false);
  }

  return (
    <div>
      <Head>
        <title>Bookings</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">My Bookings</h1>
      {showAlertSuccess && (
        <div>
          <div className="alert alert-success shadow-lg">
            <div>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="stroke-current flex-shrink-0 h-6 w-6"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span>{alertMessage}</span>
            </div>
            <div className="flex-none">
              <button onClick={onClose} className="btn btn-sm">
                Ok
              </button>
            </div>
          </div>
          <br />
        </div>
      )}
      {showAlertError && (
        <div>
          <div className="alert alert-error shadow-lg">
            <div>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="stroke-current flex-shrink-0 h-6 w-6"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span>{alertMessage}</span>
            </div>
            <div className="flex-none">
              <button onClick={onClose} className="btn btn-sm">
                Ok
              </button>
            </div>
          </div>
          <br />
        </div>
      )}
      <BookingsTable bookings={bookings} onEdit={onEdit} onDelete={onDelete} />
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
    const { n, skip, d } = context.query as {
      n: string | undefined;
      skip: string | undefined;
      d: string | undefined;
    };

    const direction = d == "DESC" || d == "ASC" ? d : "ASC";

    const bookings = await getBookings(session, {
      n: parseInt(n ?? "10") ?? 10,
      skip: parseInt(skip ?? "0") ?? 0,
      direction,
      start: Math.floor(new Date().getTime() / 1000),
    });

    return {
      props: {
        bookings,
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
