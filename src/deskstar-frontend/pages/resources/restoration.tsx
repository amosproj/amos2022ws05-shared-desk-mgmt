import React from "react";
import Head from "next/head";
import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState } from "react";
import { authOptions } from "../api/auth/[...nextauth]";
import { unstable_getServerSession } from "next-auth";
import { toast } from "react-toastify";

import BuildingResourceTable from "../../components/resources/BuildingResourceTable";
import DeskResourceTable from "../../components/resources/DeskResourceTable";
import DeskTypeResourceTable from "../../components/resources/DeskTypeResourceTable";
import FloorResourceTable from "../../components/resources/FloorResourceTable";
import RoomResourceTable from "../../components/resources/RoomResourceTable";
import {
  getBuildings,
  getDesks,
  getDeskTypes,
  getFloors,
  getRooms,
  restoreBuilding,
  restoreDesk,
  restoreDeskType,
  restoreFloor,
  restoreRoom,
  ResourceResponse,
  getAllFloors,
  getAllRooms,
  getAllDesks,
} from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { IRoom } from "../../types/room";
import FilterListbox from "../../components/FilterListbox";

export default function DeletedRessourceOverview({
  deletedBuildings,
  deletedDeskTypes,
}: {
  deletedBuildings: IBuilding[];
  deletedDeskTypes: IDeskType[];
}) {
  const { data: session } = useSession();
  const [calledRouter, setCalledRouter] = useState(false);
  const router = useRouter();

  const resourceOptions = [
    "Buildings",
    "Floors",
    "Rooms",
    "Desks",
    "Desk types",
  ];
  const [selectedResourceOption, setSelectedResourceOption] = useState<
    string | null
  >("Buildings");

  const [buildings, setBuildings] = useState<IBuilding[]>(deletedBuildings);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [desks, setDesks] = useState<IDesk[]>([]);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>(deletedDeskTypes);

  const onRestoreBuildingsUpdate = async (
    selectedBuilding: IBuilding
  ): Promise<void> => {
    if (!session) return;

    selectedBuilding.isMarkedForDeletion = false;
    const result = await restoreBuilding(session, selectedBuilding);
    if (result.response == ResourceResponse.Success) {
      toast.success(`Building ${selectedBuilding.buildingName} restored.`);
      setBuildings(
        buildings.filter(
          (building) => building.buildingId != selectedBuilding.buildingId
        )
      );
    } else {
      console.error(result.message);
      toast.error(
        `Building ${selectedBuilding.buildingName} could not be restored!`
      );
    }
  };

  const onRestoreFloorsUpdate = async (
    selectedFloor: IFloor
  ): Promise<void> => {
    if (!session) return;

    selectedFloor.isMarkedForDeletion = false;
    const result = await restoreFloor(session, selectedFloor);
    if (result.response == ResourceResponse.Success) {
      toast.success(`Floor ${selectedFloor.floorName} restored.`);
      setFloors(
        floors.filter((floor) => floor.floorId != selectedFloor.floorId)
      );
    } else {
      console.error(result.message);
      toast.error(
        `Floor ${selectedFloor.floorName} could not be restored! Make sure you have restored the corresponding resources (Building, ...) before restoring this resource.`
      );
    }
  };

  const onRestoreRoomsUpdate = async (selectedRoom: IRoom): Promise<void> => {
    if (!session) return;

    selectedRoom.isMarkedForDeletion = false;
    const result = await restoreRoom(session, selectedRoom);
    if (result.response == ResourceResponse.Success) {
      toast.success(`Room ${selectedRoom.roomName} restored!`);
      setRooms(rooms.filter((room) => room.roomId != selectedRoom.roomId));
    } else {
      console.error(result.message);
      toast.error(
        `Room ${selectedRoom.roomName} could not be restored! Make sure you have restored the corresponding resources (Building, ...) before restoring this resource.`
      );
    }
  };

  const onRestoreDesksUpdate = async (selectedDesk: IDesk): Promise<void> => {
    if (!session) return;

    selectedDesk.isMarkedForDeletion = false;
    const result = await restoreDesk(session, selectedDesk);
    if (result.response == ResourceResponse.Success) {
      toast.success(`Desk ${selectedDesk.deskName} restored.`);
      setDesks(desks.filter((desk) => desk.deskName != selectedDesk.deskName));
    } else {
      console.error(result.message);
      toast.error(
        `Desk ${selectedDesk.deskName} could not be restored! Make sure you have restored the corresponding resources (Building, ...) before restoring this resource.`
      );
    }
  };

  const onRestoreDeskTypesUpdate = async (
    selectedDeskType: IDeskType
  ): Promise<void> => {
    if (!session) return;

    selectedDeskType.isMarkedForDeletion = false;
    const result = await restoreDeskType(session, selectedDeskType);
    if (result.response == ResourceResponse.Success) {
      toast.success(`Desktype ${selectedDeskType.deskTypeName} restored.`);
      setDeskTypes(
        deskTypes.filter(
          (deskType) => deskType.deskTypeName != selectedDeskType.deskTypeName
        )
      );
    } else {
      console.error(result.message);
      toast.error(
        `Desktype ${selectedDeskType.deskTypeName} could not be restored!`
      );
    }
  };
  const onSelectedTypeChange = (option: string | null): void => {
    //TODO: Fetch data is empty
    if (option !== null) {
      setSelectedResourceOption(option);
      loadData(option);
    }
  };
  const loadData = async (option: string): Promise<void> => {
    if (!session || !selectedResourceOption) return;
    let deletedResource;
    if (option === resourceOptions[0]) {
      deletedResource = await getBuildings(session);
      deletedResource = deletedResource.filter(
        (resource: IBuilding) => resource.isMarkedForDeletion
      );
      setBuildings(deletedResource);
    } else if (option === resourceOptions[1]) {
      deletedResource = await getAllFloors(session);
      deletedResource = deletedResource.filter(
        (resource: IFloor) => resource.isMarkedForDeletion
      );
      setFloors(deletedResource);
    } else if (option === resourceOptions[2]) {
      deletedResource = await getAllRooms(session);
      deletedResource = deletedResource.filter(
        (resource: IRoom) => resource.isMarkedForDeletion
      );
      setRooms(deletedResource);
    } else if (option === resourceOptions[3]) {
      deletedResource = await getAllDesks(session);
      deletedResource = deletedResource.filter(
        (resource: IDesk) => resource.isMarkedForDeletion
      );
      setDesks(deletedResource);
    } else {
      deletedResource = await getDeskTypes(session);
      deletedResource = deletedResource.filter(
        (resource: IDeskType) => resource.isMarkedForDeletion
      );
      setDeskTypes(deletedResource);
    }
  };

  if (!session?.user?.isAdmin) {
    //TODO: Add loading animation
    return <div>Loading...</div>;
  }

  return (
    <>
      <Head>
        <title>Archived Resources</title>
      </Head>
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-center my-10">
          Archived Resources
        </h1>

        <div className="flex">
          <FilterListbox
            items={resourceOptions}
            selectedItem={selectedResourceOption}
            setSelectedItem={onSelectedTypeChange}
            getName={(resourceOption) =>
              resourceOption
                ? `Resource: ${resourceOption}`
                : "Pick a resource type"
            }
            getKey={(resourceOption) => resourceOption}
          />
        </div>
      </div>
      {selectedResourceOption === "Desk types" && (
        <>
          {deskTypes.length === 0 && (
            <p className="text-center text-xl">No archived Desk Types</p>
          )}
          {deskTypes.length > 0 && (
            <DeskTypeResourceTable
              deskTypes={deskTypes}
              onRestoreUpdate={onRestoreDeskTypesUpdate}
            />
          )}
        </>
      )}
      {selectedResourceOption === "Desks" && (
        <>
          {desks.length === 0 && (
            <p className="text-center text-xl">No archived Desks</p>
          )}
          {desks.length > 0 && (
            <DeskResourceTable
              desks={desks}
              onRestoreUpdate={onRestoreDesksUpdate}
            />
          )}
        </>
      )}
      {selectedResourceOption === "Rooms" && (
        <>
          {rooms.length === 0 && (
            <p className="text-center text-xl">No archived Rooms</p>
          )}
          {rooms.length > 0 && (
            <RoomResourceTable
              rooms={rooms}
              onRestoreUpdate={onRestoreRoomsUpdate}
            />
          )}
        </>
      )}
      {selectedResourceOption === "Floors" && (
        <>
          {floors.length === 0 && (
            <p className="text-center text-xl">No archived Floors</p>
          )}
          {floors.length > 0 && (
            <FloorResourceTable
              floors={floors}
              onRestoreUpdate={onRestoreFloorsUpdate}
            />
          )}
        </>
      )}

      {selectedResourceOption === "Buildings" && (
        <>
          {buildings.length === 0 && (
            <p className="text-center text-xl">No archived Buildings</p>
          )}
          {buildings.length > 0 && (
            <BuildingResourceTable
              buildings={buildings}
              onRestoreUpdate={onRestoreBuildingsUpdate}
            />
          )}
        </>
      )}
    </>
  );
}

export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (!session)
    return {
      redirect: {
        destination: "/login",
        permanent: false,
      },
    };
  try {
    const buildings = await getBuildings(session);
    const deskTypes = await getDeskTypes(session);
    return {
      props: {
        deletedBuildings: buildings.filter(
          (building: IBuilding) => building.isMarkedForDeletion
        ),
        deletedDeskTypes: deskTypes.filter(
          (deskType: IDeskType) => deskType.isMarkedForDeletion
        ),
      },
    };
  } catch (error) {
    console.error(error);
    return {
      redirect: {
        destination: "/500",
        permanent: false,
      },
    };
  }
};
