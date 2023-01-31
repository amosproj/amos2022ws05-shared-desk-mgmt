import { Session } from "next-auth";
import { URLSearchParams } from "next/dist/compiled/@edge-runtime/primitives/url";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { CreateBuildingDto } from "../../types/models/CreateBuildingDto";
import { CreateDeskDto } from "../../types/models/CreateDeskDto";
import { CreateDeskTypeDto } from "../../types/models/CreateDeskTypeDto";
import { CreateFloorDto } from "../../types/models/CreateFloorDto";
import { CreateRoomDto } from "../../types/models/CreateRoomDto";
import { UpdateBuildingDto } from "../../types/models/UpdateBuildingDto";
import { UpdateDeskDto } from "../../types/models/UpdateDeskDto";
import { UpdateDeskTypeDto } from "../../types/models/UpdateDeskTypeDto";
import { UpdateFloorDto } from "../../types/models/UpdateFloorDto";
import { UpdateRoomDto } from "../../types/models/UpdateRoomDto";
import { IRoom } from "../../types/room";
import { BACKEND_URL } from "./constants";

export interface IResourceResult {
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
 * Lists all floors
 * @param session The user session
 * @returns All floors
 * @throws Error containing status code and/or error message
 */
export async function getAllFloors(session: Session): Promise<IFloor[]> {
  const response = await fetch(BACKEND_URL + `/resources/floors`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

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
 * Lists all rooms
 * @param session The user session
 * @returns All rooms
 * @throws Error containing status code and/or error message
 */
export async function getAllRooms(session: Session): Promise<IRoom[]> {
  const response = await fetch(BACKEND_URL + `/resources/rooms`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  const data = await response.json();
  return data;
}

/**
 * Lists all available desks of a room
 * @param session The user session
 * @param roomId The room id
 * @param startTime optional
 * @param endTime optional
 * @returns All available desks
 * @throws Error containing status code and/or error message
 */
export async function getDesks(
  session: Session,
  roomId: string,
  startTime?: number,
  endTime?: number
): Promise<IDesk[]> {
  const params = new URLSearchParams();
  if (startTime) params.append("start", startTime.toString());
  if (endTime) params.append("end", endTime.toString());

  const response = await fetch(
    BACKEND_URL + `/resources/rooms/${roomId}/desks?${params.toString()}`,
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
 * Lists all desks
 * @param session The user session
 * @returns All desks
 * @throws Error containing status code and/or error message
 */
export async function getAllDesks(session: Session): Promise<IDesk[]> {
  const response = await fetch(BACKEND_URL + `/resources/desks`, {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

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
): Promise<IResourceResult> {
  const b = JSON.stringify(createBuildingDto);
  const response = await fetch(BACKEND_URL + "/resources/buildings", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: IResourceResult;
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
export async function updateBuilding(
  session: Session,
  updateBuildingDto: UpdateBuildingDto,
  building: IBuilding
): Promise<IResourceResult> {
  const b = JSON.stringify(updateBuildingDto);
  const response = await fetch(
    BACKEND_URL + "/resources/buildings/" + building.buildingId,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.accessToken}`,
      },
      body: b,
    }
  );
  let result: IResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    console.log(body);
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IBuilding,
      message: `Success! Updated building '${building.buildingName}'`,
    };
  }

  return result;
}
export async function updateFloor(
  session: Session,
  updateFloorDto: UpdateFloorDto,
  floor: IFloor
): Promise<IResourceResult> {
  const b = JSON.stringify(updateFloorDto);
  const response = await fetch(
    BACKEND_URL + "/resources/floors/" + floor.floorId,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.accessToken}`,
      },
      body: b,
    }
  );
  let result: IResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    console.log(body);
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IFloor,
      message: `Success! Updated floor '${floor.floorName}'`,
    };
  }

  return result;
}
export async function updateRoom(
  session: Session,
  updateRoomDto: UpdateRoomDto,
  room: IRoom
): Promise<IResourceResult> {
  const b = JSON.stringify(updateRoomDto);
  const response = await fetch(
    BACKEND_URL + "/resources/rooms/" + room.roomId,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.accessToken}`,
      },
      body: b,
    }
  );
  let result: IResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    console.log(body);
    const parsed = JSON.parse(body);
    const room = parsed as IRoom;
    room.building = parsed["buildingName"];
    room.floor = parsed["floorName"];
    result = {
      response: ResourceResponse.Success,
      data: room,
      message: `Success! Updated room '${room.roomName}'`,
    };
  }

  return result;
}
export async function updateDesk(
  session: Session,
  updateDeskDto: UpdateDeskDto,
  desk: IDesk
): Promise<IResourceResult> {
  const b = JSON.stringify(updateDeskDto);
  const response = await fetch(
    BACKEND_URL + "/resources/desks/" + desk.deskId,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.accessToken}`,
      },
      body: b,
    }
  );
  let result: IResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    console.log(body);
    const parsed = JSON.parse(body);
    const desk = parsed as IDesk;
    desk.deskTyp = parsed["deskTypeName"];
    result = {
      response: ResourceResponse.Success,
      data: desk,
      message: `Success! Updated desk '${desk.deskName}'`,
    };
  }

  return result;
}
export async function updateDeskType(
  session: Session,
  updateDeskTypeDto: UpdateDeskTypeDto,
  deskType: IDeskType
): Promise<IResourceResult> {
  const b = JSON.stringify(updateDeskTypeDto);
  const response = await fetch(
    BACKEND_URL + "/resources/desktypes/" + deskType.deskTypeId,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session.accessToken}`,
      },
      body: b,
    }
  );
  let result: IResourceResult;
  const body = await response.text();

  if (response.status !== 200) {
    result = {
      response: ResourceResponse.Error,
      message: body || "An error occured.",
    };
  } else {
    console.log(body);
    result = {
      response: ResourceResponse.Success,
      data: JSON.parse(body) as IDeskType,
      message: `Success! Updated desk type '${deskType.deskTypeName}'`,
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
): Promise<IResourceResult> {
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

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const response = await fetch(
    BACKEND_URL + `/resources/buildings/${buildingId}`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const b = JSON.stringify(createFloorDto);
  const response = await fetch(BACKEND_URL + "/resources/floors", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: IResourceResult;
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
): Promise<IResourceResult> {
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

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const response = await fetch(BACKEND_URL + `/resources/floors/${floorId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const b = JSON.stringify(createRoomDto);
  const response = await fetch(BACKEND_URL + "/resources/rooms", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: IResourceResult;
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
): Promise<IResourceResult> {
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

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const response = await fetch(BACKEND_URL + `/resources/rooms/${roomId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const b = JSON.stringify(createDeskTypeDto);
  const response = await fetch(BACKEND_URL + "/resources/desktypes", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: IResourceResult;
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
): Promise<IResourceResult> {
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

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const response = await fetch(
    BACKEND_URL + `/resources/desktypes/${deskTypeId}`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${session.accessToken}`,
      },
    }
  );

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const b = JSON.stringify(createDeskDto);
  const response = await fetch(BACKEND_URL + "/resources/desks", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });

  let result: IResourceResult;
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
): Promise<IResourceResult> {
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

  let result: IResourceResult;
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
): Promise<IResourceResult> {
  const response = await fetch(BACKEND_URL + `/resources/desks/${deskId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  let result: IResourceResult;
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
