import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../../components/BookingsTable";
import { IBooking } from "../../types/booking";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBookings } from "../../lib/api/BookingService";
import { useRouter } from "next/router";
import { useState } from "react";
import Paginator from "../../components/Paginator";

const DEFAULT_N = 10;

export default function Bookings({
  bookings,
  amountOfBookings,
}: {
  bookings: IBooking[];
  amountOfBookings: number;
}) {
  const router = useRouter();
  const [currentPage, setCurrentPage] = useState(0);

  const refreshData = (newPageNumber: number) => {
    setCurrentPage(newPageNumber);
    const path = `/bookings?page=${newPageNumber}`;
    router.push(path);
  };

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

  if (session) {
    const { page } = context.query as {
      page: string | undefined;
    };

    const direction = "ASC";

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
  } else {
    return {
      props: {
        bookings: [],
        amountOfBookings: 0,
      },
    };
  }
};
