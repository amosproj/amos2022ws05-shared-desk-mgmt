export interface IBooking {
  bookingId: string;
  userId: string;
  deskName: string;
  room: string;
  floor: string;
  building: string;
  location: string;
  timestamp: string;
  startTime: string;
  endTime: string;
}

export interface GetBookingsResponse {
  amountOfBookings: Number;
  bookings: IBooking[];
}
