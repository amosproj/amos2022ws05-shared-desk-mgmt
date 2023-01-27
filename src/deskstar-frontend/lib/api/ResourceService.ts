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

/**
 * Lists buildings associated to company of given usersession
 * @param session The user session
 * @returns The list of company buildings
 * @throws Error containing status code and/or error message
 */
export async function getBuildings(session: Session): Promise<IBuilding[]> {
  const response = await fetch(BACKEND_URL + "/resources/buildings", {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();
  return data;
}

/**
 * Lists all floors of a building
 * @param session The user session
 * @param buildingId The building id
 * @returns All floors of `buildingId`
 * @throws Error containing status code and/or error message
 */
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

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();
  return data;
}

/**
 * Lists all rooms of a floor
 * @param session The user session
 * @param floorId The floor id
 * @returns All rooms of `floorId`
 * @throws Error containing status code and/or error message
 */
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

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();
  return data;
}

/**
 * Lists all available desks of a room
 * @param session The user session
 * @param roomId The room id
 * @param startTime
 * @param endTime
 * @returns All available desks
 * @throws Error containing status code and/or error message
 */
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

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();
  return data;
}

/**
 * Lists all defined desk types associated to usersessions company
 * @param session The user session
 * @returns All defined desk types associated to usersessions company
 * @throws Error containing status code and/or error message
 */
export async function getDeskTypes(session: Session): Promise<IDeskType[]> {
  const response = await fetch(BACKEND_URL + `/resources/desktypes`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();
  const resDeskTypes = data.map((e: any) => {
    return {
      deskTypeId: e["deskTypeId"],
      deskTypeName: e["deskTypeName"],
    };
  });

  return resDeskTypes;
}

/**
 * Creates a building
 * @param session The user session
 * @param createBuildingDto the building data for the post request
 * @returns
 */
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

/**
 * Restore a building
 * @param session The user session
 * @param restoreBuilding The building Object to restore
 * @returns
 */
export async function restoreBuilding(
  session: Session,
  restoreBuilding: IBuilding
): Promise<ICreateResourceResult> {
  var buildingId = restoreBuilding.buildingId;
  const response = await fetch(
    BACKEND_URL + `/resources/buildings/restore/${buildingId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Restored building with id '${buildingId}'`,
    };
  }

  return result;
}

/**
 * Delete a building
 * @param session The user session
 * @param deskTypeId The id of the building to delete
 * @returns
 */
export async function deleteBuilding(
  session: Session,
  buildingId: string
): Promise<ICreateResourceResult> {
  const response = await fetch(
    BACKEND_URL + `/resources/buildings/${buildingId}`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Deleted building with id '${buildingId}'`,
    };
  }

  return result;
}

/**
 * Creates a floor
 * @param session The user session
 * @param createFloorDto The floor data for the post request
 * @returns
 */
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

/**
 * Restore a floor
 * @param session The user session
 * @param restoreFloor The floor Object to restore
 * @returns
 */
export async function restoreFloor(
  session: Session,
  restoreFloor: IFloor
): Promise<ICreateResourceResult> {
  var floorId = restoreFloor.floorId;
  const response = await fetch(
    BACKEND_URL + `/resources/floors/restore/${floorId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Restored floor with id '${floorId}'`,
    };
  }

  return result;
}

/**
 * Delete a floor
 * @param session The user session
 * @param deskTypeId The id of the floor to delete
 * @returns
 */
export async function deleteFloor(
  session: Session,
  floorId: string
): Promise<ICreateResourceResult> {
  const response = await fetch(BACKEND_URL + `/resources/floors/${floorId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
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
      message: `Success! Deleted floor with id '${floorId}'`,
    };
  }

  return result;
}

/**
 * Creates a room
 * @param session The user session
 * @param createRoomDto The room data for the post request
 * @returns
 */
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

/**
 * Restore a room
 * @param session The user session
 * @param restoreRoom The room Object to restore
 * @returns
 */
export async function restoreRoom(
  session: Session,
  restoreRoom: IRoom
): Promise<ICreateResourceResult> {
  var roomId = restoreRoom.roomId;
  const response = await fetch(
    BACKEND_URL + `/resources/rooms/restore/${roomId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Restored room with id '${roomId}'`,
    };
  }

  return result;
}

/**
 * Delete a room
 * @param session The user session
 * @param deskTypeId The id of the room to delete
 * @returns
 */
export async function deleteRoom(
  session: Session,
  roomId: string
): Promise<ICreateResourceResult> {
  const response = await fetch(BACKEND_URL + `/resources/rooms/${roomId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
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
      message: `Success! Deleted room with id '${roomId}'`,
    };
  }

  return result;
}

/**
 * Creates a new desk type
 * @param session The user session
 * @param createDeskTypeDto The desktype data for the post request
 * @returns
 */
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

/**
 * Restore a deskType
 * @param session The user session
 * @param restoreDeskType The deskType Object to restore
 * @returns
 */
export async function restoreDeskType(
  session: Session,
  restoreDeskType: IDeskType
): Promise<ICreateResourceResult> {
  var desktypeId = restoreDeskType.deskTypeId;
  const response = await fetch(
    BACKEND_URL + `/resources/desktypes/restore/${desktypeId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Restored desk type with id '${desktypeId}'`,
    };
  }

  return result;
}

/**
 * Delete a deskType
 * @param session The user session
 * @param deskTypeId The id of the deskType to delete
 * @returns
 */
export async function deleteDeskType(
  session: Session,
  deskTypeId: string
): Promise<ICreateResourceResult> {
  const response = await fetch(
    BACKEND_URL + `/resources/desktypes/${deskTypeId}`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Deleted desktype with id '${deskTypeId}'`,
    };
  }

  return result;
}

/**
 * Creates a desk
 * @param session The user session
 * @param createDeskDto The desk data for the post request
 * @returns
 */
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

/**
 * Restore a desk
 * @param session The user session
 * @param restoreDesk The desk Object to restore
 * @returns
 */
export async function restoreDesk(
  session: Session,
  restoreDesk: IDesk
): Promise<ICreateResourceResult> {
  var deskId = restoreDesk.deskId;
  const response = await fetch(
    BACKEND_URL + `/resources/desks/restore/${deskId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

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
      message: `Success! Restored desk with id '${deskId}'`,
    };
  }

  return result;
}

/**
 * Delete a desk
 * @param session The user session
 * @param deskTypeId The id of the desk to delete
 * @returns
 */
export async function deleteDesk(
  session: Session,
  deskId: string
): Promise<ICreateResourceResult> {
  const response = await fetch(BACKEND_URL + `/resources/desks/${deskId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
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
      message: `Success! Deleted desk with id'${deskId}'`,
    };
  }

  return result;
}
