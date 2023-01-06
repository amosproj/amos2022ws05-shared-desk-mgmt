import { Session } from "next-auth";
import { IBooking } from "../../types/booking";
import { BACKEND_URL } from "./constants";

type QueryOptions = {
  n?: number;
  skip?: number;
  direction?: "ASC" | "DESC";
  start?: EpochTimeStamp;
  end?: EpochTimeStamp;
};

function getParams(queryOptions: QueryOptions) {
  const mapTable: [any, string][] = [
    [queryOptions.n, "n"],
    [queryOptions.skip, "skip"],
    [queryOptions.direction, "direction"],
    [queryOptions.start, "start"],
    [queryOptions.end, "end"],
  ];

  const params = new URLSearchParams();

  mapTable.forEach((val) => {
    if (val[0]) {
      params.append(val[1], val[0]);
    }
  });

  return params;
}

export async function getBookings(
  session: Session,
  queryOptions: QueryOptions
): Promise<IBooking[]> {
  if (!session.user) return [];

  const params = getParams(queryOptions);
  const response = await fetch(BACKEND_URL + `/bookings/range?${params}`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (response.status !== 200) {
    return [];
  }

  const data = await response.json();

  const bookings: IBooking[] = data.map((val: any) => {
    return {
      bookingId: val.timestamp,
      userId: session.user.id,
      room: val.roomName,
      floor: val.floorName,
      building: val.buildingName,
      location: "N/A",
      ...val,
    };
  });

  return bookings;
}

export async function createBooking(
  session: Session,
  deskId: string,
  startTime: Date,
  endTime: Date
) {
  const response = await fetch(BACKEND_URL + "/bookings", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: JSON.stringify({
      deskId,
      startTime,
      endTime,
    }),
  });
  console.log(
    JSON.stringify({
      deskId,
      startTime,
      endTime,
    })
  );
  console.log(await response.status);
  if (response.status !== 200) {
    let text = await response.text();
    // take away the quotes
    text = text.substring(1, text.length - 1);
    console.log(text);
    switch (text) {
      case "User not found":
        return "User not found";
      case "Desk not found":
        return "The desk you are trying to book does not exist";
      case "Time slot not available":
        return "The time slot you are trying to book is not available";
      default:
        return "An unknown error occurred";
    }
  } else {
    return "success";
  }
}

// create function for deleting booking. BookingId is passed as query parameter
export async function deleteBooking(session: Session, bookingId: string) {
  const response = await fetch(BACKEND_URL + `/bookings/${bookingId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  // check if response is 200. If not return error message
  if ((await response.status) !== 200) {
    let text = await response.text();
    text = text.substring(1, text.length - 1);
    console.log(text);
    switch (text) {
      case "User not found":
        return "User not found";
      case "Booking not found":
        return "The booking you are trying to delete does not exist";
      case "You are not allowed to delete this booking":
        return "You are not allowed to delete this booking";
      default:
        return "An unknown error occurred";
    }
  } else {
    return "success";
  }
}
