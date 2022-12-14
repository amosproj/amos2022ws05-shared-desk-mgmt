import Head from "next/head";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
import { authOptions } from "../api/auth/[...nextauth]";
import { unstable_getServerSession } from "next-auth";
import ResourceManagementTable from "../../components/ResourceManagementTable";
import DropDownFilter from "../../components/DropDownFilter";
import { IDesk } from "../../types/desk";
import { IRoom } from "../../types/room";
import { IBuilding } from "../../types/building";
import { ILocation } from "../../types/location";
import { IFloor } from "../../types/floor";
import {
  getBuildings,
  getDesks,
  getFloors,
  getRooms,
} from "../../lib/api/ResourceService";
import { GetServerSideProps } from "next";

const ResourceOverview = ({
  buildings: origBuildings,
}: {
  buildings: IBuilding[];
}) => {
  let { data: session } = useSession();

  const locations: ILocation[] = origBuildings.map((building) => ({
    locationName: building.location,
  }));

  const router = useRouter();

  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [desks, setDesks] = useState<IDesk[]>([]);

  async function onSelectedLocationChange(selectedLocations: ILocation[]) {
    let buildings = origBuildings.filter((building) =>
      selectedLocations.some((location) => {
        return location.locationName === building.location;
      })
    );

    setBuildings(buildings);
  }

  async function onSelectedBuildingChange(selectedBuildings: IBuilding[]) {
    const promises = await Promise.all(
      selectedBuildings.map(async (building) => {
        if (!session) {
          return [];
        }

        const resFloors = await getFloors(session, building.buildingId);

        return resFloors;
      })
    );

    setFloors(promises.flat());
  }

  async function onSelectedFloorChange(selectedFloors: IFloor[]) {
    const promises = await Promise.all(
      selectedFloors.map(async (floor) => {
        if (!session) {
          return [];
        }

        const resRooms = await getRooms(session, floor.floorID);
        return resRooms;
      })
    );

    setRooms(promises.flat());
  }

  async function onSelectedRoomChange(selectedRooms: IRoom[]) {
    const promises = await Promise.all(
      selectedRooms.map(async (room) => {
        if (!session) {
          return [];
        }

        const resDeskType = await getDesks(
          session,
          room.roomId,
          new Date().getTime(),
          new Date().getTime()
        );

        return resDeskType;
      })
    );

    const desks = promises.flat();
    const filteredDesks = desks.filter((desk) => desk.bookings.length === 0);
    setDesks(filteredDesks);
  }

  // redirect if user is not admin as page is only accessible for admins
  useEffect(() => {
    if (session && !session?.user.isAdmin) {
      // redirect to homepage
      router.push({
        pathname: "/",
      });
    }
  }, [router, session]);

  const onEdit = async (desk: IDesk): Promise<void> => {
    //TODO: Implement
    console.log(`Editing desk ${desk.deskId}...`);
  };

  const onDelete = async (desk: IDesk): Promise<void> => {
    //TODO: Implement
    console.log(`Deleting desk ${desk.deskId}...`);
  };

  return (
    <>
      <Head>
        <title>Resources Overview</title>
      </Head>

      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-left my-10">
          Resources Overview
        </h1>
        <button
          type="button"
          className="btn btn-secondary bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light"
          onClick={() => {}}
        >
          Add Resource
        </button>
      </div>
      <DropDownFilter
        title="Locations"
        getItemName={(location) => location.locationName}
        options={locations}
        setSelectedOptions={onSelectedLocationChange}
      />

      {buildings.length > 0 && (
        <DropDownFilter
          title="Buildings"
          getItemName={(building) => building.buildingName}
          options={buildings}
          setSelectedOptions={onSelectedBuildingChange}
        />
      )}

      {floors.length > 0 && (
        <DropDownFilter
          title="Floors"
          getItemName={(floor) => floor.floorName}
          options={floors}
          setSelectedOptions={onSelectedFloorChange}
        />
      )}

      {rooms.length > 0 && (
        <DropDownFilter
          title="Rooms"
          getItemName={(room) => room.roomName}
          options={rooms}
          setSelectedOptions={onSelectedRoomChange}
        />
      )}

      <div className="my-4"></div>

      {desks.length > 0 && (
        <ResourceManagementTable
          onEdit={onEdit}
          onDelete={onDelete}
          desks={desks}
        />
      )}

      {buildings.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a location</span>
          </div>
        </div>
      )}
      {!(buildings.length == 0) && floors.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a building</span>
          </div>
        </div>
      )}
      {!(floors.length == 0) && rooms.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a floor</span>
          </div>
        </div>
      )}
      {!(rooms.length == 0) && desks.length == 0 && (
        <div className="toast">
          <div className="alert alert-info">
            <span>Please select a room</span>
          </div>
        </div>
      )}
    </>
  );
};

export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (session) {
    const buildings = await getBuildings(session);

    return {
      props: {
        buildings,
      },
    };
  }

  return {
    props: {
      buildings: [],
    },
  };
};

export default ResourceOverview;
