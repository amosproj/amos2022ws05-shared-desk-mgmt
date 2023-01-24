import { Session } from "next-auth";
import { IBooking, GetBookingsResponse } from "../../types/booking";
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

/**
 * Gets list of bookings associated with user session
 * @param session The associated user session
 * @param queryOptions The Query parameter for the request
 * @returns The total amount of bookings and list of bookings according to `queryOptions`
 * @throws Error containing status code and/or error message
 */
export async function getBookings(
  session: Session,
  queryOptions: QueryOptions
): Promise<GetBookingsResponse> {
  if (!session.user)
    return {
      amountOfBookings: 0,
      bookings: [],
    };

  const params = getParams(queryOptions);
  const response = await fetch(BACKEND_URL + `/bookings?${params}`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();

  const bookingsResponse: GetBookingsResponse = {
    amountOfBookings: data.amountOfBookings,
    bookings: data?.bookings.map((val: any) => {
      return {
        bookingId: val.timestamp,
        userId: session.user.id,
        room: val.roomName,
        floor: val.floorName,
        building: val.buildingName,
        location: "N/A",
        ...val,
      };
    }),
  };

  return bookingsResponse;
}

/**
 * Creates a user booking
 * @param session The associated user session
 * @param deskId The desk id associated to the booking
 * @param startTime Start time of the booking
 * @param endTime End time of the booking
 * @returns The response object
 * @throws Error containing status code and/or error message
 */
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

  if (!response.ok) {
    let text = await response.text();
    // take away the quotes
    text = text.substring(1, text.length - 1);

    const textToErrorMessage: { [key: string]: string } = {
      "User not found": "User not found",
      "Desk not found": "The desk you are trying to book does not exist",
      "Time slot not available":
        "The time slot you are trying to book is not available",
    };

    throw Error(
      `${response.status} ${textToErrorMessage[text] ?? response.statusText}`
    );
  }

  return response;
}

/**
 * Deletes a booking associated to an user
 * @param session The associated user session
 * @param bookingId The bookings id
 * @returns The response object
 * @throws Error containing status code and/or error message
 */
export async function deleteBooking(session: Session, bookingId: string) {
  const response = await fetch(BACKEND_URL + `/bookings/${bookingId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  // check if response is 200. If not return error message
  if (!response.ok) {
    let text = await response.text();
    text = text.substring(1, text.length - 1);

    const textToErrorMessage: { [key: string]: string } = {
      "User not found": "User not found",
      "Booking not found":
        "The booking you are trying to delete does not exist",
      "You are not allowed to delete this booking":
        "You are not allowed to delete this booking",
    };

    throw Error(
      `${response.status} ${textToErrorMessage[text] ?? response.statusText}`
    );
  }
  return response;
}

/**
 * Update start and end time for a given booking associated to an user
 * @param session the user session
 * @param bookingId The bookings id
 * @param startTime new start time for given booking
 * @param endTime new end time for given booking
 * @returns The updated booking
 * @throws Error containing status code and/or error message
 */
export async function updateBooking(
  session: Session,
  bookingId: string,
  startTime: string,
  endTime: string
) {
  const response = await fetch(BACKEND_URL + `/bookings/${bookingId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: JSON.stringify({
      startTime,
      endTime,
    }),
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  //TODO: fix this
  //return await response.json();
  return response;
}
