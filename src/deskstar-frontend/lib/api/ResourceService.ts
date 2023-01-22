import { Session } from "next-auth";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { CreateBuildingDto } from "../../types/models/CreateBuildingDto";
import { CreateDeskDto } from "../../types/models/CreateDeskDto";
import { CreateDeskTypeDto } from "../../types/models/CreateDeskTypeDto";
import { CreateFloorDto } from "../../types/models/CreateFloorDto";
import { CreateRoomDto } from "../../types/models/CreateRoomDto";
import { IRoom } from "../../types/room";
import { BACKEND_URL } from "./constants";

export interface ICreateResourceResult {
  response: ResourceResponse;
  message: string;
  data?: Object;
}

export enum ResourceResponse {
  Error,
  Success,
}

export async function getBuildings(session: Session): Promise<IBuilding[]> {
  const response = await fetch(BACKEND_URL + "/resources/buildings", {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (response.status !== 200) {
    console.log(response.status);
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
    console.log(response.status);
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
    console.log(response.status);
    console.log("Error fetching rooms");
    return [];
  }

  const data = await response.json();

  return data;
}

export async function getDesks(
  session: Session,
  roomId: string,
  startTime: number,
  endTime: number
): Promise<IDesk[]> {
  const response = await fetch(
    BACKEND_URL +
      `/resources/rooms/${roomId}/desks?start=${startTime}&end=${endTime}`,
    {
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

  if (response.status !== 200) {
    console.log(response.status);
    console.log("Error fetching desks");
    return [];
  }

  const data = await response.json();

  return data;
}

export async function getDeskTypes(session: Session): Promise<IDeskType[]> {
  const response = await fetch(BACKEND_URL + `/resources/desktypes`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (response.status !== 200) {
    console.log(response.status);
    console.log("Error fetching desks");
    return [];
  }

  const data = await response.json();
  const resDeskTypes = data.map((e: any) => {
    return {
      deskTypeId: e["deskTypeId"],
      deskTypeName: e["deskTypeName"],
    };
  });

  return resDeskTypes;
}

export async function createBuilding(
  session: Session,
  createBuildingDto: CreateBuildingDto
): Promise<ICreateResourceResult> {
  const b = JSON.stringify(createBuildingDto);
  const response = await fetch(BACKEND_URL + "/resources/buildings", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: ICreateResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IBuilding,
      message: `Success! Created building '${createBuildingDto.buildingName}'`,
    };
  }

  return result;
}

export async function deleteBuilding(
  session: Session,
  buildingId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/resources/buildings/${buildingId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export async function createFloor(
  session: Session,
  createFloorDto: CreateFloorDto
): Promise<ICreateResourceResult> {
  const b = JSON.stringify(createFloorDto);
  const response = await fetch(BACKEND_URL + "/resources/floors", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: ICreateResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IFloor,
      message: `Success! Created floor '${createFloorDto.floorName}'`,
    };
  }

  return result;
}

export async function deleteFloor(
  session: Session,
  floorId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/resources/floors/${floorId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export async function createRoom(
  session: Session,
  createRoomDto: CreateRoomDto
): Promise<ICreateResourceResult> {
  const b = JSON.stringify(createRoomDto);
  const response = await fetch(BACKEND_URL + "/resources/rooms", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: ICreateResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IRoom,
      message: `Success! Created room '${createRoomDto.roomName}'`,
    };
  }

  return result;
}

export async function deleteRoom(
  session: Session,
  roomId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/resources/rooms/${roomId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export async function createDeskType(
  session: Session,
  createDeskTypeDto: CreateDeskTypeDto
): Promise<ICreateResourceResult> {
  const b = JSON.stringify(createDeskTypeDto);
  const response = await fetch(BACKEND_URL + "/resources/desktypes", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: ICreateResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IDeskType,
      message: `Success! Created type '${createDeskTypeDto.deskTypeName}'`,
    };
  }

  return result;
}

export async function deleteDeskType(
  session: Session,
  deskTypeId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/resources/desktypes/${deskTypeId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export async function createDesk(
  session: Session,
  createDeskDto: CreateDeskDto
): Promise<ICreateResourceResult> {
  const b = JSON.stringify(createDeskDto);
  const response = await fetch(BACKEND_URL + "/resources/desks", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: ICreateResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IDesk,
      message: `Success! Created desk '${createDeskDto.deskName}'`,
    };
  }

  return result;
}

export async function deleteDesk(
  session: Session,
  deskId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/resources/desks/${deskId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}
