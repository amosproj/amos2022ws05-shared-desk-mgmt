import { Session } from "next-auth";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IFloor } from "../../types/floor";
import { IRoom } from "../../types/room";
import { BACKEND_URL } from "./constants";

export async function getLocations() {
  //   const response = await fetch(BACKEND_URL + "/resources/locations");
}

export async function getBuildings(session: Session): Promise<IBuilding[]> {
  const response = await fetch(BACKEND_URL + "/resources/buildings", {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (response.status !== 200) {
    console.log("Error fetching buildings");
    return [];
  }

  const data = await response.json();

  return data;
}

export async function getFloors(
  session: Session,
  buildingId: string
): Promise<IFloor[]> {
  const response = await fetch(
    BACKEND_URL + `/resources/buildings/${buildingId}/floors`,
    {
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

  if (response.status !== 200) {
    console.log("Error fetching floors");
    return [];
  }

  const data = await response.json();

  return data;
}

export async function getRooms(
  session: Session,
  floorId: string
): Promise<IRoom[]> {
  const response = await fetch(
    BACKEND_URL + `/resources/floors/${floorId}/rooms`,
    {
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

  if (response.status !== 200) {
    console.log("Error fetching rooms");
    return [];
  }

  const data = await response.json();

  return data;
}

export async function getDesks(
  session: Session,
  roomId: string,
  startTime: string,
  endTime: string
): Promise<IDesk[]> {
  const response = await fetch(
    BACKEND_URL + `/resources/rooms/${roomId}/desks`,
    {
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

  if (response.status !== 200) {
    console.log("Error fetching desks");
    return [];
  }

  const data = await response.json();
  console.log(data);
  return data;
}
