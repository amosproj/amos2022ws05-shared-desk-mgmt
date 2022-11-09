import { IBooking } from "../types/booking";

const BookingsTable = ({ bookings }: { bookings: IBooking[] }) => {
  return (
    <table>
      <thead>
        <tr>
          <th>Desk Id</th>
          <th>Start Date</th>
          <th>Start Time</th>
          <th>End Date</th>
          <th>End Time</th>
        </tr>
      </thead>
      <tbody>
        {bookings.map((booking: IBooking) => (
          <BookingTableEntry key={booking.timestamp} booking={booking} />
        ))}
      </tbody>
    </table>
  );
};

const BookingTableEntry = ({ booking }: { booking: IBooking }) => {
  const startDate = booking.startTime.split("T")[0];
  const startTime = booking.startTime.split("T")[1].replace("Z", "");
  const endDate = booking.endTime.split("T")[0];
  const endTime = booking.endTime.split("T")[1].replace("Z", "");

  return (
    <tr>
      <td className="text-center">{booking.deskId}</td>
      <td className="text-center">{startDate}</td>
      <td className="text-center">{startTime}</td>
      <td className="text-center">{endDate}</td>
      <td className="text-center">{endTime}</td>
    </tr>
  );
};

export default BookingsTable;
