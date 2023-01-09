import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../../components/BookingsTable";
import { IBooking } from "../../types/booking";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBookings, deleteBooking } from "../../lib/api/BookingService";
import { toast } from "react-toastify";
import { useSession } from "next-auth/react";

export default function Bookings({ bookings }: { bookings: IBooking[] }) {
  const { data: session } = useSession();

  async function onDelete(booking: IBooking) {
    if (session == null) return;

    try {
      let response = await deleteBooking(session, booking.bookingId);

      if (response == "success") {
        toast.success("Booking successfully deleted!");

        let index = bookings.indexOf(booking);
        if (index > -1) {
          bookings.splice(index, 1);
        }
        return;
      } else {
        toast.error(response);
      }
    } catch (error) {
      toast.error("Error calling Server:" + error);
    }
  }

  const onEdit = (booking: IBooking) => {
    //TODO: implement
    toast.success(`Pressed edit on ${booking.bookingId}`);
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
