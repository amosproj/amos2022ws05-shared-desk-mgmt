import { IBooking } from "../types/booking";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import { UpdateBookingModal } from "./UpdateBookingModal";

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
  const startDate = booking.startTime.split("T")[0];
  const startTime = booking.startTime.split("T")[1].replace("Z", "");
  const endDate = booking.endTime.split("T")[0];
  const endTime = booking.endTime.split("T")[1].replace("Z", "");

  return (
    <tr className="hover">
      <td className="text-center">{booking.deskName}</td>
      <td className="text-center">{booking.room}</td>
      <td className="text-center">{booking.building}</td>
      <td className="text-center">{startDate}</td>
      <td className="text-center">{startTime}</td>
      <td className="text-center">{endDate}</td>
      <td className="text-center">{endTime}</td>
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
