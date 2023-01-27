import { IBooking } from "../types/booking";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import { UpdateBookingModal } from "./UpdateBookingModal";
import dayjs, { Dayjs } from "dayjs";
import { useEffect, useState } from "react";

const BookingsTable = ({
  bookings,
  onEdit,
  onDelete,
  onCheckIn,
}: {
  bookings: IBooking[];
  onEdit?: (booking: IBooking, startTime: Date, endTime: Date) => void;
  onDelete?: Function;
  onCheckIn?: Function;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full ">
        <thead className="dark:text-black">
          <tr>
            <th className="bg-deskstar-green-light text-center">Desk</th>
            <th className="bg-deskstar-green-light text-center">Room</th>
            <th className="bg-deskstar-green-light text-center">Building</th>
            <th className="bg-deskstar-green-light text-center">Start Date</th>
            <th className="bg-deskstar-green-light text-center">Start Time</th>
            <th className="bg-deskstar-green-light text-center">End Date</th>
            <th className="bg-deskstar-green-light text-center">End Time</th>
            {onEdit && <th className="bg-deskstar-green-light"></th>}
            {onDelete && <th className="bg-deskstar-green-light"></th>}
            {onCheckIn && <th className="bg-deskstar-green-light"></th>}
          </tr>
        </thead>
        <tbody>
          {bookings.map((booking: IBooking) => (
            <BookingTableEntry
              key={booking.bookingId}
              booking={booking}
              onEdit={onEdit}
              onDelete={onDelete}
              onCheckIn={onCheckIn}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
};

const BookingTableEntry = ({
  booking,
  onEdit,
  onDelete,
  onCheckIn,
}: {
  booking: IBooking;
  onEdit?: Function;
  onDelete?: Function;
  onCheckIn?: Function;
}) => {
  const [start, setStart] = useState<Dayjs>();

  const [end, setEnd] = useState<Dayjs>();

  // Needed, because dayjs cannot be run on the server
  useEffect(() => {
    setStart(
      dayjs(booking.startTime, {
        utc: true,
      })
    );

    setEnd(
      dayjs(booking.endTime, {
        utc: true,
      })
    );
  }, [booking.startTime, booking.endTime]);

  return (
    <tr className="hover">
      <td className="text-center">
        <>
          {booking.usesDeletedDesk && (
            <div className="indicator">
              <span
                title="This booking uses a deleted desk!"
                className="indicator-item badge badge-error"
              >
                !
              </span>
              <div className="grid w-10 h-10 place-items-center">
                {booking.deskName}
              </div>
            </div>
          )}
          {!booking.usesDeletedDesk && <>{booking.deskName}</>}
        </>
      </td>
      <td className="text-center">{booking.room}</td>
      <td className="text-center">{booking.building}</td>
      <td className="text-center">{start?.format("DD.MM.YYYY")}</td>
      <td className="text-center">{start?.format("HH:mm")}</td>
      <td className="text-center">{end?.format("DD.MM.YYYY")}</td>
      <td className="text-center">{end?.format("HH:mm")}</td>
      {onEdit && (
        <td className="p-0">
          <label
            htmlFor={`my-update-booking-${booking.bookingId}-modal`}
            className="btn btn-ghost"
          >
            <FaEdit />
          </label>
          <UpdateBookingModal
            id={`my-update-booking-${booking.bookingId}-modal`}
            booking={booking}
            onUpdate={onEdit}
          />
        </td>
      )}
      {onDelete && (
        <td className="p-0">
          <button className="btn btn-ghost" onClick={() => onDelete(booking)}>
            <FaTrashAlt color="red" />
          </button>
        </td>
      )}
      {onCheckIn && (
        <td className="text-right">
          <button className="btn btn-ghost" onClick={() => onCheckIn(booking)}>
            Check in
          </button>
        </td>
      )}
    </tr>
  );
};

export default BookingsTable;
