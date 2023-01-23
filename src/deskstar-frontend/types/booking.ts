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
  usesDeletedDesk: boolean;
}

export interface GetBookingsResponse {
  amountOfBookings: Number;
  bookings: IBooking[];
}
