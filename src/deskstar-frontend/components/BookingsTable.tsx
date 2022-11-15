import { IBooking } from "../types/booking";

const BookingsTable = ({ bookings }: { bookings: IBooking[] }) => {
  return (
    <div className="overflow-x-auto">
    <table className="table table-zebra w-full">
      <thead>
        <tr>
          <th className="bg-deskstar-green-light text-center">Desk</th>
          <th className="bg-deskstar-green-light text-center">Room</th>
          <th className="bg-deskstar-green-light text-center">Building</th>
          <th className="bg-deskstar-green-light text-center">Start Date</th>
          <th className="bg-deskstar-green-light text-center">Start Time</th>
          <th className="bg-deskstar-green-light text-center">End Date</th>
          <th className="bg-deskstar-green-light text-center">End Time</th>
        </tr>
      </thead>
      <tbody>
        {bookings.map((booking: IBooking) => (
          <BookingTableEntry key={booking.bookingId} booking={booking} />
        ))}
      </tbody>
    </table>
    </div>
  );
};

const BookingTableEntry = ({ booking }: { booking: IBooking }) => {
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
    </tr>
  );
};

export default BookingsTable;
