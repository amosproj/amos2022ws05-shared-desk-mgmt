import { IBooking } from "../types/booking";
import { FaTrashAlt, FaEdit } from "react-icons/fa";

const BookingsTable = ({
  bookings,
  onEdit,
  onDelete,
}: {
  bookings: IBooking[];
  onEdit?: (booking: IBooking) => void;
  onDelete?: Function;
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
          </tr>
        </thead>
        <tbody>
          {bookings.map((booking: IBooking) => (
            <BookingTableEntry
              key={booking.bookingId}
              booking={booking}
              onEdit={onEdit}
              onDelete={onDelete}
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
}: {
  booking: IBooking;
  onEdit?: Function;
  onDelete?: Function;
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
          <button className="btn btn-ghost" onClick={() => onEdit(booking)}>
            <FaEdit />
          </button>
        </td>
      )}
      {onDelete && (
        <td className="p-0">
          <button className="btn btn-ghost" onClick={() => onDelete(booking)}>
            <FaTrashAlt color="red" />
          </button>
        </td>
      )}
    </tr>
  );
};

export default BookingsTable;
