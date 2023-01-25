import React from "react";
import Head from "next/head";
import { GetServerSideProps } from "next";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
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
  editBuilding,
  editDesk,
  editDeskType,
  editFloor,
  editRoom,
  ResourceResponse,
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
    const result = await editBuilding(session, selectedBuilding);
    if (result.response == ResourceResponse.Success) {
      toast.success(result.message);
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
    const result = await editFloor(session, selectedFloor);
    if (result.response == ResourceResponse.Success) {
      toast.success(result.message);
      setFloors(
        floors.filter((floor) => floor.floorId != selectedFloor.floorId)
      );
    } else {
      console.error(result.message);
      toast.error(`Floor ${selectedFloor.floorName} could not be restored!`);
    }
  };

  const onRestoreRoomsUpdate = async (selectedRoom: IRoom): Promise<void> => {
    if (!session) return;

    selectedRoom.isMarkedForDeletion = false;
    const result = await editRoom(session, selectedRoom);
    if (result.response == ResourceResponse.Success) {
      toast.success(result.message);
      setRooms(rooms.filter((room) => room.roomId != selectedRoom.roomId));
    } else {
      console.error(result.message);
      toast.error(`Room ${selectedRoom.roomName} could not be restored!`);
    }
  };

  const onRestoreDesksUpdate = async (selectedDesk: IDesk): Promise<void> => {
    if (!session) return;

    selectedDesk.isMarkedForDeletion = false;
    const result = await editDesk(session, selectedDesk);
    if (result.response == ResourceResponse.Success) {
      toast.success(result.message);
      setDesks(desks.filter((desk) => desk.deskName != selectedDesk.deskName));
    } else {
      console.error(result.message);
      toast.error(`Desk ${selectedDesk.deskName} could not be restored!`);
    }
  };

  const onRestoreDeskTypesUpdate = async (
    selectedDeskType: IDeskType
  ): Promise<void> => {
    if (!session) return;

    selectedDeskType.isMarkedForDeletion = false;
    const result = await editDeskType(session, selectedDeskType);
    if (result.response == ResourceResponse.Success) {
      toast.success(result.message);
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

  if (!session?.user?.isAdmin) {
    //TODO: Add loading animation
    return <div>Loading...</div>;
  }

  return (
    <>
      <Head>
        <title>Archived Ressources</title>
      </Head>
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-center my-10">
          Archived Ressources
        </h1>

        <div className="flex">
          <FilterListbox
            items={resourceOptions}
            selectedItem={selectedResourceOption}
            setSelectedItem={setSelectedResourceOption}
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
          <DeskTypeResourceTable
            deskTypes={deskTypes}
            onRestoreUpdate={onRestoreDeskTypesUpdate}
          />
        </>
      )}
      {selectedResourceOption === "Desks" && (
        <>
          <DeskResourceTable
            desks={desks}
            onRestoreUpdate={onRestoreDesksUpdate}
          />
        </>
      )}
      {selectedResourceOption === "Rooms" && (
        <>
          <RoomResourceTable
            rooms={rooms}
            onRestoreUpdate={onRestoreRoomsUpdate}
          />
        </>
      )}
      {selectedResourceOption === "Floors" && (
        <>
          <FloorResourceTable
            floors={floors}
            onRestoreUpdate={onRestoreFloorsUpdate}
          />
        </>
      )}

      {selectedResourceOption === "Buildings" && (
        <>
          <BuildingResourceTable
            buildings={buildings}
            onRestoreUpdate={onRestoreBuildingsUpdate}
          />
        </>
      )}
    </>
  );
}

//TODO: delete this when using backend data instead of mockup
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
