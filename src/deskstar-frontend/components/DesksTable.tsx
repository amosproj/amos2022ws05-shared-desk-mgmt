import { IDesk } from "../types/desk";
import React, { useState } from "react";
import dayjs from "dayjs";
import { useSession } from "next-auth/react";

const DesksTable = ({
  desks,
  onBook,
}: {
  desks: IDesk[];
  onBook: Function;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            <th className="w-1/4 bg-secondary text-left">Desk</th>
            <th className="bg-secondary text-left">Type</th>
            <th className="bg-secondary"></th>
          </tr>
        </thead>
        <tbody>
          {desks.map((desk: IDesk) => (
            <DeskTableEntry key={desk.deskId} desk={desk} onBook={onBook} />
          ))}
        </tbody>
      </table>
    </div>
  );
};

const DeskTableEntry = ({
  desk,
  onBook,
}: {
  desk: IDesk;
  onBook: Function;
}) => {
  const [buttonText, setButtonText] = useState("Book");

  const { data: session } = useSession();

  const isBooked = desk.bookings.length > 0;

  return (
    <tr className="hover">
      <td className="text-left font-bold">{desk.deskName}</td>
      <td className="text-left">{desk.deskTyp}</td>
      <td className="text-right">
        {isBooked && (
          <button className="btn btn-success" disabled>
            Booked by{" "}
            {desk.bookings[0].userId == session?.user.id
              ? "you"
              : desk.bookings[0].userName}
            <br />
            {dayjs(desk.bookings[0].startTime, {
              utc: true,
            }).format("HH:mm")}{" "}
            -{" "}
            {dayjs(desk.bookings[0].endTime, {
              utc: true,
            }).format("HH:mm")}
          </button>
        )}
        {!isBooked && (
          <button
            className="btn btn-success"
            onClick={(event) => onBook(event, desk, setButtonText)}
          >
            {buttonText}
          </button>
        )}
      </td>
    </tr>
  );
};

export default DesksTable;
