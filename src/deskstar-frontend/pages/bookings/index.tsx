import Head from "next/head";
import { GetServerSideProps } from "next";
import BookingsTable from "../../components/BookingsTable";
import { IBooking } from "../../types/booking";
import { unstable_getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]";
import { getBookings, deleteBooking } from "../../lib/api/BookingService";
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

  const [showAlertSuccess, setShowAlertSuccess] = useState(false);
  const [showAlertError, setShowAlertError] = useState(false);
  const [alertMessage, setAlertMessage] = useState("");

  const { data: session } = useSession();

  const refreshData = (newPageNumber: number) => {
    setCurrentPage(newPageNumber);
    const path = `/bookings?page=${newPageNumber}`;
    router.push(path);
  };

  const onDelete = async (booking: IBooking) => {
    if (session == null) return;
    //TODO: implement
    console.log(`Pressed delete on ${booking.bookingId}`);

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
  };
  
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
