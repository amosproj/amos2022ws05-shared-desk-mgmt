import React from "react";
import Head from "next/head";
import { GetServerSideProps } from "next";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
import { getUsers, editUser } from "../../lib/api/UserService";
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
  >("Desks");

  const [buildings, setBuilding] = useState<IBuilding[]>(deletedBuildings);
  const [floors, setFloor] = useState<IFloor[]>([]);
  const [rooms, setRoom] = useState<IRoom[]>([]);
  const [desks, setDesk] = useState<IDesk[]>([]);
  const [deskTypes, setDeskType] = useState<IDeskType[]>(deletedDeskTypes);

  const onRestoreBuildingsUpdate = async (
    selectedBuilding: IBuilding
  ): Promise<void> => {
    //
  };

  const onRestoreFloorsUpdate = async (
    selectedFloor: IFloor
  ): Promise<void> => {
    //
  };

  const onRestoreRoomsUpdate = async (selectedRoom: IRoom): Promise<void> => {
    //
  };

  const onRestoreDesksUpdate = async (selectedDesk: IDesk): Promise<void> => {
    //
  };

  const onRestoreDeskTypesUpdate = async (
    selectedDeskType: IDeskType
  ): Promise<void> => {
    //
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
