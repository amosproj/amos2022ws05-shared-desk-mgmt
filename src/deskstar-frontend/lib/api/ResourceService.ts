import { Session } from "next-auth";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IDeskType } from "../../types/desktypes";
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
    BACKEND_URL + `/resources/rooms/${roomId}/desks?start=${startTime}&end=${endTime}`,
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

export async function getDeskTypes(
  session: Session,
): Promise<IDeskType[]> {
  const response = await fetch(
    BACKEND_URL + `/resources/desktypes`,
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
  const resDeskTypes = data.map((e: any) => {
    return {
      typeId: e["deskTypeId"],
      typeName: e["deskTypeName"]
    }
  });
  return resDeskTypes;
}
type CreateRoomDto = {
  floorId: string;
  roomName: string;
};
export async function createRoom(
  session: Session,
  createRoomDto: CreateRoomDto,
) {
  const b = JSON.stringify(createRoomDto);
  const response = await fetch(BACKEND_URL + "/resources/rooms", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });
  if (response.status !== 200) {
    const text = await response.text();
    return JSON.parse(text);
  } else {
    return `success! Added room '${createRoomDto.roomName}'`;
  }
}
type CreateFloorDto = {
  buildingId: string;
  floorName: string;
};
export async function createFloor(
  session: Session,
  createFloorDto: CreateFloorDto,
) {
  const b = JSON.stringify(createFloorDto);
  const response = await fetch(BACKEND_URL + "/resources/floors", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });
  if (response.status !== 200) {
    const text = await response.text();
    return JSON.parse(text);
  } else {
    return `success! Added floor '${createFloorDto.floorName}'`;
  }
}
type CreateBuildingDto = {
  buildingName: string;
  location: string;
};
export async function createBuilding(
  session: Session,
  createBuildingDto: CreateBuildingDto,
) {
  const b = JSON.stringify(createBuildingDto);
  const response = await fetch(BACKEND_URL + "/resources/buildings", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });
  if (response.status !== 200) {
    const text = await response.text();
    return JSON.parse(text);
  } else {
    return `success! Added building '${createBuildingDto.buildingName}'`;
  }
}
type CreateDeskTypeDto = {
  deskTypeName: string;
};
export async function createDeskType(
  session: Session,
  createDeskDto: CreateDeskTypeDto,
) {
  const b = JSON.stringify(createDeskDto);
  const response = await fetch(BACKEND_URL + "/resources/desktypes", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });
  if (response.status !== 200) {
    const text = await response.text();
    return JSON.parse(text);
  } else {
    return `success! Added desk type '${createDeskDto.deskTypeName}'`;
  }
}
type CreateDeskDto = {
  roomId: string;
  deskName: string;
  deskTypeId: string;
};
export async function createDesk(
  session: Session,
  createDeskDto: CreateDeskDto,
) {
  const b = JSON.stringify(createDeskDto);
  const response = await fetch(BACKEND_URL + "/resources/desks", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: b,
  });
  if (response.status !== 200) {
    const text = await response.text();
    return JSON.parse(text);
  } else {
    return `success! Added desk '${createDeskDto.deskName}'`;
  }
}

async function CreateEnity(entityType: string, entityName: string, body: string, session: Session) {
  const response = await fetch(BACKEND_URL + `/resources/${entityType}s`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session.accessToken}`,
    },
    body: body,
  });
  if (response.status !== 200) {
    const text = await response.text();
    return JSON.parse(text);
  } else {
    return `success! Added ${entityType} '${entityName}'`;
  }
}
