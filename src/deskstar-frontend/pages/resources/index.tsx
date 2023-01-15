import { GetServerSideProps } from "next";
import { unstable_getServerSession } from "next-auth";
import { useSession } from "next-auth/react";
import Head from "next/head";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import AddResourceModal from "../../components/AddResourceModal";
import DropDownFilter from "../../components/DropDownFilter";
import FilterListbox from "../../components/FilterListbox";
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
  getRooms
} from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { ILocation } from "../../types/location";
import { IRoom } from "../../types/room";
import { authOptions } from "../api/auth/[...nextauth]";

const ResourceOverview = ({
  buildings: origBuildings,
  deskTypes: origDeskTypes,
}: {
  buildings: IBuilding[];
  deskTypes: IDeskType[];
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
  const [desktypes, setDeskTypes] = useState<IDeskType[]>([]);

  const resourceOptions = ["Buildings", "Floors", "Rooms", "Desks", "Desk types"];
  const [selectedResourceOption, setSelectedResourceOption] = useState<string | null>("Desks");
  const [isFetching, setIsFetching] = useState<boolean>(false);

  async function onSelectedLocationChange(selectedLocations: ILocation[]) {
    let buildings = origBuildings.filter((building) =>
      selectedLocations.some((location) => {
        return location.locationName === building.location;
      })
    );

    setBuildings(buildings);
  }

  async function onSelectedBuildingChange(selectedBuildings: IBuilding[]) {
    setIsFetching(true);
    const promises = await Promise.all(
      selectedBuildings.map(async (building) => {
        if (!session) {
          return [];
        }

        const resFloors = await getFloors(session, building.buildingId);
        const enrichedFloors = resFloors.map((floor) => {
          floor.buildingName = building.buildingName;
          floor.location = building.location;
          return floor;
        })
        return enrichedFloors;
      })
    );

    setFloors(promises.flat());setIsFetching(true);
    setIsFetching(false);
  }

  async function onSelectedFloorChange(selectedFloors: IFloor[]) {
    setIsFetching(true);
    const promises = await Promise.all(
      selectedFloors.map(async (floor) => {
        if (!session) {
          return [];
        }

        const resRooms = await getRooms(session, floor.floorId);
        const enrichedRooms = resRooms.map((room) => {
          room.building = floor.buildingName;
          room.location = floor.location;
          room.floor = floor.floorName;
          return room;
        })
        return enrichedRooms;
      })
    );

    setRooms(promises.flat());
    setIsFetching(false);
  }

  async function onSelectedRoomChange(selectedRooms: IRoom[]) {
    setIsFetching(true);
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
    setIsFetching(false);
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

  const onEdit = async (resource: object): Promise<void> => {
    //TODO: Implement
    toast.success(`Editing resource...`);
  };

  const onDelete = async (resource: object): Promise<void> => {
    //TODO: Implement
    toast.success(`Deleting resource...`);
  };

  const getIndex = (resourceName: string | null): number => {
    if (resourceName == null)
      return -1;
    return resourceOptions.findIndex((v, i, obj) => resourceName === v);
  }

  return (
    <>
      {isFetching && <progress className="progress progress-info h-2"></progress>}
      {!isFetching && <div className="h-6"></div>}
      
      <Head>
        <title>Resources Overview</title>
      </Head>

      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-left mb-10 mt-5">
          Resources Overview
        </h1>
        <div className="flex">
          <FilterListbox
            items={resourceOptions}
            selectedItem={selectedResourceOption}
            setSelectedItem={setSelectedResourceOption}
            getName={(resourceOption) =>
              resourceOption ? `Resource: ${resourceOption}` : "Pick a resource type"
            }
            getKey={(resourceOption) => resourceOption}
          />
          <a
            href="#create-resource-modal"
            type="button"
            className="btn text-black btn-secondary bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light ml-2"
            onClick={() => { }}>Add Resource</a>
          <AddResourceModal buildings={origBuildings} deskTypes={origDeskTypes} />
        </div>
      </div>


      {selectedResourceOption !== "Desk types" && (
        <>
          <DropDownFilter
            title="Locations"
            getItemName={(location) => location.locationName}
            options={locations}
            setSelectedOptions={onSelectedLocationChange}
          />

          {buildings.length > 0 && getIndex(selectedResourceOption) > 0 && (
            <DropDownFilter
              title="Buildings"
              getItemName={(building) => building.buildingName}
              options={buildings}
              setSelectedOptions={onSelectedBuildingChange}
            />
          )}

          {floors.length > 0 && getIndex(selectedResourceOption) > 1 && (
            <DropDownFilter
              title="Floors"
              getItemName={(floor) => floor.floorName}
              options={floors}
              setSelectedOptions={onSelectedFloorChange}
            />
          )}

          {rooms.length > 0 && getIndex(selectedResourceOption) > 2 && (
            <DropDownFilter
              title="Rooms"
              getItemName={(room) => room.roomName}
              options={rooms}
              setSelectedOptions={onSelectedRoomChange}
            />
          )}
        </>
      )
      }

      {selectedResourceOption !== "Desk types" && (
        <>
          <div className="my-4"></div>
        </>
      )
      }
      {selectedResourceOption === "Desk types" && (
        <>
          {origDeskTypes.length > 0 && (
            <DeskTypeResourceTable
              onEdit={onEdit}
              onDelete={onDelete}
              deskTypes={origDeskTypes}
            />
          )}
        </>
      )
      }
      {selectedResourceOption === "Desks" && (<>
        {desks.length > 0 && (
          <DeskResourceTable
            onEdit={onEdit}
            onDelete={onDelete}
            desks={desks}
          />
        )}
      </>)}
      {selectedResourceOption === "Rooms" && (<>
        {rooms.length > 0 && (
          <RoomResourceTable
            onEdit={onEdit}
            onDelete={onDelete}
            rooms={rooms}
          />
        )}
      </>)}
      {selectedResourceOption === "Floors" && (<>
        {floors.length > 0 && (
          <FloorResourceTable
            onEdit={onEdit}
            onDelete={onDelete}
            floors={floors}
          />
        )}
      </>)}


      {selectedResourceOption === "Buildings" && (<>
        {buildings.length > 0 && (
          <BuildingResourceTable
            onEdit={onEdit}
            onDelete={onDelete}
            buildings={buildings}
          />
        )}
      </>)}


      {buildings.length == 0 && (
        <div className="toast">
          <div className="alert bg-deskstar-green-dark text-black">
            <span>Please select a location</span>
          </div>
        </div>
      )}
      {!(buildings.length == 0) && floors.length == 0 && (
        <div className="toast">
          <div className="alert bg-deskstar-green-dark text-black">
            <span>Please select a building</span>
          </div>
        </div>
      )}
      {!(floors.length == 0) && rooms.length == 0 && (
        <div className="toast">
          <div className="alert bg-deskstar-green-dark text-black">
            <span>Please select a floor</span>
          </div>
        </div>
      )}
      {!(rooms.length == 0) && desks.length == 0 && (
        <div className="toast">
          <div className="alert bg-deskstar-green-dark text-black">
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
    const deskTypes = await getDeskTypes(session);
    return {
      props: {
        buildings,
        deskTypes,
      },
    };
  }

  return {
    props: {
      buildings: [],
      deskTypes: [],
    },
  };
};

export default ResourceOverview;
