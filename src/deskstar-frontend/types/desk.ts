export interface IDesk {
  deskId: string;
  deskName: string;
  deskTyp: string;
  roomId: string;
  roomName: string;
  floorId: string;
  floorName: string;
  buildingId: string;
  buildingName: string;
  location: string;
  bookings: IDeskBooking[];
  isMarkedForDeletion: boolean;
}

export interface IDeskBooking {
  bookingId: string;
  userId: string;
  endTime: string;
  startTime: string;
}
