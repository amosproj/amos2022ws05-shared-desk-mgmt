import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../components/BookingsTable";
import { bookings as mockBookings } from "../bookings";
import { IBooking } from "../types/booking";

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

export const getServerSideProps: GetServerSideProps = async () => {
  //TODO: fetch here upcoming bookings from backend
  const sortedBookings = mockBookings.sort((a: IBooking, b: IBooking) =>
    a.startTime.localeCompare(b.startTime)
  );
  return {
    props: {
      bookings: sortedBookings,
    },
  };
};
