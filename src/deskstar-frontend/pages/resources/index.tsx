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
import BuildingResourceEditModal from "../../components/resources/BuildingResourceEditModal";
import BuildingResourceTable from "../../components/resources/BuildingResourceTable";
import DeskResourceEditModal from "../../components/resources/DeskResourceEditModal";
import DeskResourceTable from "../../components/resources/DeskResourceTable";
import DeskTypeResourceEditModal from "../../components/resources/DeskTypeResourceEditModal";
import DeskTypeResourceTable from "../../components/resources/DeskTypeResourceTable";
import FloorResourceEditModal from "../../components/resources/FloorResourceEditModal";
import FloorResourceTable from "../../components/resources/FloorResourceTable";
import RoomResourceEditModal from "../../components/resources/RoomResourceEditModal";
import RoomResourceTable from "../../components/resources/RoomResourceTable";
import {
  getBuildings,
  getDesks,
  getDeskTypes,
  getFloors,
  getRooms,
  updateBuilding,
  updateDesk,
  updateDeskType,
  updateFloor,
  updateRoom,
} from "../../lib/api/ResourceService";
import { UpdateBuildingDto } from "../../types/models/UpdateBuildingDto";
import { IBuilding } from "../../types/building";
import { IDesk } from "../../types/desk";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { ILocation } from "../../types/location";
import { IRoom } from "../../types/room";
import { authOptions } from "../api/auth/[...nextauth]";
import { UpdateFloorDto } from "../../types/models/UpdateFloorDto";
import { UpdateRoomDto } from "../../types/models/UpdateRoomDto";
import { UpdateDeskTypeDto } from "../../types/models/UpdateDeskTypeDto";
import { UpdateDeskDto } from "../../types/models/UpdateDeskDto";

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
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>(origDeskTypes);

  const [editDeskType, setEditDeskType] = useState<IDeskType>();
  const [isEditDeskTypeModalOpen, setIsEditDeskTypeModalOpen] = useState<boolean>(false);

  const [editDesk, setEditDesk] = useState<IDesk>();
  const [isEditDeskModalOpen, setIsEditDeskModalOpen] = useState<boolean>(false);

  const [editRoom, setEditRoom] = useState<IRoom>();
  const [isEditRoomModalOpen, setIsEditRoomModalOpen] = useState<boolean>(false);

  const [editFloor, setEditFloor] = useState<IFloor>();
  const [isEditFloorModalOpen, setIsEditFloorModalOpen] = useState<boolean>(false);

  const [editBuilding, setEditBuilding] = useState<IBuilding>();
  const [isEditBuildingModalOpen, setIsEditBuildingModalOpen] = useState<boolean>(false);

  const [deskTypeNameModal, setDeskTypeNameModal] = useState<string>("");
  const [deskNameModal, setDeskNameModal] = useState<string>("");
  const [roomNameModal, setRoomNameModal] = useState<string>("");
  const [floorNameModal, setFloorNameModal] = useState<string>("");
  const [buildingNameModal, setBuildingNameModal] = useState<string>("");
  const [locationModal, setLocationModal] = useState<string>("");


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
  const [isFetching, setIsFetching] = useState<boolean>(false);

  async function onSelectedLocationChange(selectedLocations: ILocation[]) {
    let buildings = origBuildings.filter((building) =>
      selectedLocations.some((location) => {
        return location.locationName === building.location;
      })
    );

    setBuildings(buildings);
  }

  function stopFetchingAnimation() {
    setTimeout(() => {
      setIsFetching(false);
    }, 1500);
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
        });
        return enrichedFloors;
      })
    );

    setFloors(promises.flat());
    setIsFetching(true);
    stopFetchingAnimation();
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
        });
        return enrichedRooms;
      })
    );

    setRooms(promises.flat());
    stopFetchingAnimation();
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
    stopFetchingAnimation();
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

  const openDeskTypeEditModal = async (deskType: IDeskType): Promise<void> => {
    setDeskTypeNameModal(deskType.deskTypeName)
    setEditDeskType(deskType);
    setIsEditDeskTypeModalOpen(true);
  };
  const openDeskEditModal = async (desk: IDesk): Promise<void> => {
    setDeskNameModal(desk.deskName);
    setEditDesk(desk);
    setIsEditDeskModalOpen(true);
  };
  const openRoomEditModal = async (room: IRoom): Promise<void> => {
    setRoomNameModal(room.roomName);
    setEditRoom(room);
    setIsEditRoomModalOpen(true);
  };
  const openFloorEditModal = async (floor: IFloor): Promise<void> => {
    setFloorNameModal(floor.floorName);
    setEditFloor(floor);
    setIsEditFloorModalOpen(true);
  };
  const openBuildingEditModal = async (building: IBuilding): Promise<void> => {
    setEditBuilding(building);
    setBuildingNameModal(building.buildingName);
    setLocationModal(building.location);
    setIsEditBuildingModalOpen(true);
  };
  const updateBuildingState = async (building: IBuilding): Promise<void> => {
    const i = buildings.findIndex((v, i) => v.buildingId == building.buildingId);
    buildings[i] = building;
    setBuildings(buildings);
  };
  const updateFloorState = async (floor: IFloor): Promise<void> => {
    const i = floors.findIndex((v, i) => v.floorId == floor.floorId);
    floors[i] = floor;
    setFloors(floors);
  };
  const updateRoomState = async (room: IRoom): Promise<void> => {
    const i = rooms.findIndex((v, i) => v.roomId == room.roomId);
    rooms[i] = room;
    setRooms(rooms);
  };
  const updateDeskState = async (desk: IDesk): Promise<void> => {
    const i = desks.findIndex((v, i) => v.deskId == desk.deskId);
    desks[i] = desk;
    setDesks(desks);
  };
  const updateDeskTypeState = async (deskType: IDeskType): Promise<void> => {
    const i = deskTypes.findIndex((v, i) => v.deskTypeId == deskType.deskTypeId);
    deskTypes[i] = deskType;
    setDeskTypes(deskTypes);
  };
  const updateBuildingInService = async (updateBuildingDto: UpdateBuildingDto, selectedBuilding: IBuilding): Promise<void> => {
    if (!session) return;
    const result = await updateBuilding(session, updateBuildingDto, selectedBuilding);
    setIsEditBuildingModalOpen(false);
    showMessageToast(result.message);
    if (result.message.includes("Success")) {
      const updatedBuilding = result.data as (IBuilding);
      updateBuildingState(updatedBuilding);
    }
  };

  const updateFloorInService = async (updateFloorDto: UpdateFloorDto, selectedFloor: IFloor): Promise<void> => {
    if (!session) return;
    const result = await updateFloor(session, updateFloorDto, selectedFloor);
    setIsEditFloorModalOpen(false);
    showMessageToast(result.message);
    if (result.message.includes("Success")) {
      const updatedFloor = result.data as (IFloor);
      updateFloorState(updatedFloor);
    }
  };
  const updateRoomInService = async (updateRoomDto: UpdateRoomDto, selectedRoom: IRoom): Promise<void> => {
    if (!session) return;
    const result = await updateRoom(session, updateRoomDto, selectedRoom);
    setIsEditRoomModalOpen(false);
    showMessageToast(result.message);
    if (result.message.includes("Success")) {
      const updatedRoom = result.data as (IRoom);
      updateRoomState(updatedRoom);
    }
  };
  const updateDeskInService = async (updateDeskDto: UpdateDeskDto, selectedDesk: IDesk): Promise<void> => {
    if (!session) return;
    const result = await updateDesk(session, updateDeskDto, selectedDesk);
    setIsEditDeskModalOpen(false);
    showMessageToast(result.message);
    if (result.message.includes("Success")) {
      const updatedDesk = result.data as (IDesk);
      updateDeskState(updatedDesk);
    }
  };
  const updateDeskTypeInService = async (updateDeskTypeDto: UpdateDeskTypeDto, selectedDeskType: IDeskType): Promise<void> => {
    if (!session) return;
    const result = await updateDeskType(session, updateDeskTypeDto, selectedDeskType);
    setIsEditDeskTypeModalOpen(false);
    showMessageToast(result.message);
    if (result.message.includes("Success")) {
      const updatedDeskType = result.data as (IDeskType);
      updateDeskTypeState(updatedDeskType);
    }
  };
  const showMessageToast = (message: string): void => {
    message.includes("Success") ? toast.success(message) : toast.warn(message);
  }

  const onDelete = async (resource: object): Promise<void> => {
    //TODO: Implement
    toast.success(`Deleting resource...`);
  };

  const getIndex = (resourceName: string | null): number => {
    if (resourceName == null) return -1;
    return resourceOptions.findIndex((v, i, obj) => resourceName === v);
  };
  if (!session) return (<></>)
  return (
    <>
      {isFetching && (
        <progress className="progress progress-info h-2"></progress>
      )}
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
              resourceOption
                ? `Resource: ${resourceOption}`
                : "Pick a resource type"
            }
            getKey={(resourceOption) => resourceOption}
          />
          <a
            href="#create-resource-modal"
            type="button"
            className="btn text-black btn-secondary bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light ml-2"
            onClick={() => { }}
          >
            Add Resource
          </a>
          <AddResourceModal
            buildings={origBuildings}
            deskTypes={origDeskTypes}
          />
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
      )}

      {selectedResourceOption !== "Desk types" && (
        <>
          <div className="my-4"></div>
        </>
      )}
      {selectedResourceOption === "Desk types" && (
        <>
          {deskTypes.length > 0 && (
            <DeskTypeResourceTable
              onEdit={openDeskTypeEditModal}
              onDelete={onDelete}
              deskTypes={deskTypes}
            />
          )}
          <DeskTypeResourceEditModal isOpen={isEditDeskTypeModalOpen} setState={setIsEditDeskTypeModalOpen} desktype={editDeskType} session={session} updateDeskType={updateDeskTypeInService} deskTypeName={deskTypeNameModal} setDeskTypeName={setDeskTypeNameModal} />
        </>
      )}
      {selectedResourceOption === "Desks" && (
        <>
          {desks.length > 0 && (
            <DeskResourceTable
              onEdit={openDeskEditModal}
              onDelete={onDelete}
              desks={desks}
            />
          )}
          <DeskResourceEditModal desk={editDesk} rooms={rooms} isOpen={isEditDeskModalOpen} setState={setIsEditDeskModalOpen} session={session} updateDesk={updateDeskInService} origBuildings={origBuildings} origDeskTypes={origDeskTypes} deskName={deskNameModal} setDeskName={setDeskNameModal} />
        </>
      )}
      {selectedResourceOption === "Rooms" && (
        <>
          {rooms.length > 0 && (
            <RoomResourceTable
              onEdit={openRoomEditModal}
              onDelete={onDelete}
              rooms={rooms}
            />
          )}
          <RoomResourceEditModal updateRoom={updateRoomInService} origBuildings={buildings} session={session} isOpen={isEditRoomModalOpen} setState={setIsEditRoomModalOpen} room={editRoom} roomName={roomNameModal} setRoomName={setRoomNameModal} />
        </>
      )}
      {selectedResourceOption === "Floors" && (
        <>
          {floors.length > 0 && (
            <FloorResourceTable
              onEdit={openFloorEditModal}
              onDelete={onDelete}
              floors={floors}
            />
          )}
          <FloorResourceEditModal isOpen={isEditFloorModalOpen} setState={setIsEditFloorModalOpen} floor={editFloor} origBuildings={origBuildings} updateFloor={updateFloorInService} session={session} floorName={floorNameModal} setFloorName={setFloorNameModal} />
        </>
      )}

      {selectedResourceOption === "Buildings" && (
        <>
          {buildings.length > 0 && (
            <BuildingResourceTable
              onEdit={openBuildingEditModal}
              onDelete={onDelete}
              buildings={buildings}
            />
          )}
          <BuildingResourceEditModal isOpen={isEditBuildingModalOpen} setState={setIsEditBuildingModalOpen} building={editBuilding} updateBuilding={updateBuildingInService} buildingName={buildingNameModal} location={locationModal} setBuildingName={setBuildingNameModal} setLocation={setLocationModal} />
        </>
      )}

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
