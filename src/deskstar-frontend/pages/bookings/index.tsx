import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../../components/BookingsTable";
import { IBooking } from "../../types/booking";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import {
  getBookings,
  deleteBooking,
  updateBooking,
} from "../../lib/api/BookingService";
import { toast } from "react-toastify";
import { useRouter } from "next/router";
import { useState } from "react";
import { useSession } from "next-auth/react";
import Paginator from "../../components/Paginator";

const DEFAULT_N = 10;

//TODO: replace notifications with toast alerts
export default function Bookings({
  bookings,
  amountOfBookings,
}: {
  bookings: IBooking[];
  amountOfBookings: number;
}) {
  const router = useRouter();
  const [currentPage, setCurrentPage] = useState(0);

  const { data: session } = useSession();

  const refreshData = (newPageNumber: number) => {
    setCurrentPage(newPageNumber);
    const path = `/bookings?page=${newPageNumber}`;
    router.push(path);
  };

  const onDelete = async (booking: IBooking) => {
    if (session == null) return;
    console.log(`Pressed delete on ${booking.bookingId}`);

    try {
      await deleteBooking(session, booking.bookingId);

      toast.success("Booking successfully deleted!");
      refreshData(currentPage);
    } catch (error) {
      toast.error(`${error}`);
    }
  };

  const onEdit = async (
    booking: IBooking,
    newStartTime: Date,
    newEndTime: Date
  ) => {
    if (session == null) return;

    const update = {
      startTime: newStartTime.toISOString(),
      endTime: newEndTime.toISOString(),
    };

    try {
      await updateBooking(
        session,
        booking.bookingId,
        update.startTime,
        update.endTime
      );

      toast.success(`Booking successfully updated.`);
      refreshData(currentPage);
    } catch (error) {
      toast.error(`${error}`);
    }
  };

  return (
    <div>
      <Head>
        <title>Bookings</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">My Bookings</h1>
      <BookingsTable bookings={bookings} onEdit={onEdit} onDelete={onDelete} />
      <div className="flex justify-center mt-10">
        <Paginator
          n={DEFAULT_N}
          total={amountOfBookings}
          currentPage={currentPage}
          onChange={refreshData}
        />
      </div>
    </div>
  );
}

export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (!session)
    return {
      redirect: {
        destination: "/login",
        permanent: false,
      },
    };

  const { page } = context.query as {
    page: string | undefined;
  };

  const direction = "DESC";

  try {
    const data = await getBookings(session, {
      n: DEFAULT_N,
      skip: parseInt(page ?? "0") * DEFAULT_N,
      direction,
      start: Math.floor(new Date().getTime() / 1000),
    });

    return {
      props: {
        bookings: data.bookings,
        amountOfBookings: data.amountOfBookings,
      },
    };
  } catch (error) {
    console.error(error);
    return {
      redirect: {
        destination: "/500",
        permanent: false,
      },
    };
  }
};
