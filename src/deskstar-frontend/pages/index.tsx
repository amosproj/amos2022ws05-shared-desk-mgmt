import { GetServerSideProps } from "next"
import { useSession} from "next-auth/react";
import Head from "next/head";
import BookingsTable from "../components/BookingsTable";
import { IBooking } from "../types/booking";
//TODO: delete this when fetching from database
import {bookings} from "../bookings";

export default function AppHome({bookings}: {bookings: IBooking[]}) {
  const { data: session } = useSession();
  
  return (
    <div>
      <Head>
        <title>Dashboard</title>
      </Head>
      <h1 className="text-3xl font-bold text-center mt-10">Hello {session?.user?.name}, welcome back to Deskstar</h1>
      <h1 className="text-2xl font-bold text-center my-10">Your latest bookings</h1>
      <BookingsTable bookings={bookings} />
    </div>
  );
}

export const getServerSideProps: GetServerSideProps = async (context) => {
  //TODO: fetch here latest bookings from backend
  return {
    props: {
      bookings: bookings
    }
  }
}
