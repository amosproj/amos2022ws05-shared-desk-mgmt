import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../../components/BookingsTable";
import { bookings as mockBookings } from "../../bookings";
import { IBooking } from "../../types/booking";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBookings } from "../../lib/api/BookingService";

export default function Bookings({ bookings }: { bookings: IBooking[] }) {
  const onDelete = (booking: IBooking) => {
    //TODO: implement
    console.log(`Pressed delete on ${booking.bookingId}`);
  };
  const onEdit = (booking: IBooking) => {
    //TODO: implement
    console.log(`Pressed edit on ${booking.bookingId}`);
  };

  return (
    <div>
      <Head>
        <title>Bookings</title>
      </Head>

      <h1 className="text-3xl font-bold text-center my-10">My Bookings</h1>
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

    const direction = d == "ASC" || d == "DESC" ? d : "DESC";

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
