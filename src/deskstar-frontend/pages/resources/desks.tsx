import { useMemo, useReducer, useState } from "react";
import Head from "next/head";
import ResourceManagementHeader from "../../components/resources/ResourceManagementHeader";
import GenericAddResourceModal from "../../components/resources/GenericAddResourceModal";
import Input from "../../components/forms/Input";
import FilterListbox from "../../components/FilterListbox";
import {
  ILocation,
  IBuilding,
  IFloor,
  IRoom,
  IDeskType,
  IDesk,
  Identifiable,
} from "../../types";
import { CreateDeskDto } from "../../types/models/CreateDeskDto";
import {
  getBuildings,
  getDesks,
  getDeskTypes,
  getFloors,
  getRooms,
} from "../../lib/api/ResourceService";
import { authOptions } from "../api/auth/[...nextauth]";
import { GetServerSideProps } from "next";
import { unstable_getServerSession } from "next-auth";
import DeskResourceTable from "../../components/resources/DeskResourceTable";
import ResourceTable from "../../components/resources/ResourceTable";
import DropDownFilter from "../../components/DropDownFilter";

type Data = {
  locations: ILocation[];
  buildings: IBuilding[];
  floors: IFloor[];
  rooms: IRoom[];
  deskTypes: IDeskType[];
  desks: IDesk[];
};

type DesksProps = {
  data: Data;
};

// Resource Table Definition
const headers = ["Desk", "Desk Type", "Room", "Floor", "Building", "Location"];
const fields = [
  (desk: IDesk) => desk.deskName,
  (desk: IDesk) => desk.deskTyp,
  (desk: IDesk) => desk.roomName,
  (desk: IDesk) => desk.floorName,
  (desk: IDesk) => desk.buildingName,
  (desk: IDesk) => desk.location,
];

export default function Desks({ data }: DesksProps) {
  const locations = data.locations;

  const [desks, setDesks] = useState(
    data.desks.map((desk) => ({
      ...desk,
      id: desk.deskId,
    }))
  );

  // Filters
  const [selectedFilterLocations, setSelectedFilterLocations] =
    useState<ILocation[]>(locations);
  const [selectedFilterBuildings, setSelectedFilterBuildings] = useState<
    IBuilding[]
  >(data.buildings);
  const [selectedFilterFloors, setSelectedFilterFloors] = useState<IFloor[]>(
    data.floors
  );
  const [selectedFilterRooms, setSelectedFilterRooms] = useState<IRoom[]>(
    data.rooms
  );
  const [selectedFilterDeskTypes, setSelectedFilterDeskTypes] = useState<
    IDeskType[]
  >(data.deskTypes);

  const buildings = useMemo(() => {
    return data.buildings.filter((building) => {
      return (
        selectedFilterLocations.findIndex(
          (location) => location.locationName === building.location
        ) !== -1
      );
    });
  }, [data.buildings, selectedFilterLocations]);
  const [floors, setFloors] = useState<IFloor[]>(data.floors);
  const [rooms, setRooms] = useState<IRoom[]>(data.rooms);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>(data.deskTypes);

  function filtered(desks: (IDesk & Identifiable)[]) {
    let filteredDesks = desks;

    // Filter by location
    filteredDesks = filteredDesks.filter((desk) => {
      return selectedFilterLocations.some(
        (location) => location.locationName === desk.location
      );
    });

    // Filter by building
    filteredDesks = filteredDesks.filter((desk) => {
      return selectedFilterBuildings.some(
        (building) => building.buildingName === desk.buildingName
      );
    });

    // Filter by floor
    filteredDesks = filteredDesks.filter((desk) => {
      return selectedFilterFloors.some(
        (floor) => floor.floorName === desk.floorName
      );
    });

    // Filter by room
    filteredDesks = filteredDesks.filter((desk) => {
      return selectedFilterRooms.some(
        (room) => room.roomName === desk.roomName
      );
    });

    // Filter by desk type
    filteredDesks = filteredDesks.filter((desk) => {
      return selectedFilterDeskTypes.some(
        (deskType) => deskType.deskTypeName === desk.deskTyp
      );
    });

    return filteredDesks;
  }

  // Modals
  const [isAddResourceOpen, setIsAddResourceOpen] = useState(false);
  const [isAddResourceLoading, setIsAddResourceLoading] = useState(false);
  const [newDesk, setNewDesk] = useState<IDesk | null>(null);

  return (
    <>
      <Head>
        <title>Desk Resource Management</title>
      </Head>

      <ResourceManagementHeader
        addAction={() => {
          setIsAddResourceOpen(true);
        }}
      />

      {/* Filter */}
      <div className="mb-4">
        <DropDownFilter
          title="Locations"
          getItemName={(location) => location.locationName}
          options={locations}
          setSelectedOptions={setSelectedFilterLocations}
          preselectedOptions={locations}
        />

        <DropDownFilter
          title="Buildings"
          getItemName={(building) => building.buildingName}
          options={buildings}
          setSelectedOptions={setSelectedFilterBuildings}
          preselectedOptions={buildings}
        />

        <DropDownFilter
          title="Floors"
          getItemName={(floor) => floor.floorName}
          options={floors}
          setSelectedOptions={setSelectedFilterFloors}
          preselectedOptions={floors}
        />

        <DropDownFilter
          title="Rooms"
          getItemName={(room) => room.roomName}
          options={rooms}
          setSelectedOptions={setSelectedFilterRooms}
          preselectedOptions={rooms}
        />
      </div>

      {/* Table */}
      <div className="flex flex-col">
        <ResourceTable
          headers={headers}
          fields={fields}
          resources={desks}
          setResources={setDesks}
          filterResources={filtered}
          editable={true}
          deletable={true}
        />
      </div>

      {/* Modals, will only be displayed if opened */}
      <AddDeskModal
        data={data}
        isOpen={isAddResourceOpen}
        setIsOpen={setIsAddResourceOpen}
        isLoading={isAddResourceLoading}
        setIsLoading={setIsAddResourceLoading}
        actionAdd={async () => {}}
      />
    </>
  );
}

type AddDeskModalProps = DesksProps & {
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
  isLoading: boolean;
  setIsLoading: (isLoading: boolean) => void;
  actionAdd: (desk: IDesk) => Promise<void>;
};

function AddDeskModal({
  isOpen,
  setIsOpen,
  isLoading,
  setIsLoading,
  data,
}: AddDeskModalProps) {
  const [newDesk, setNewDesk] = useState<CreateDeskDto>({
    deskName: "",
    deskTypeId: "",
    roomId: "",
  });

  // Selected filter attributes
  const [selectedDeskType, setSelectedDeskType] = useState<IDeskType | null>(
    null
  );
  const [selectedLocation, setSelectedLocation] = useState<ILocation | null>(
    null
  );
  const [selectedBuilding, setSelectedBuilding] = useState<IBuilding | null>(
    null
  );
  const [selectedFloor, setSelectedFloor] = useState<IFloor | null>(null);
  const [room, setRoom] = useState<IRoom | null>(null);

  return (
    <GenericAddResourceModal
      title="Add Desk"
      isOpen={isOpen}
      setIsOpen={setIsOpen}
      isLoading={isLoading}
      setIsLoading={setIsLoading}
      actionAdd={async () => {}}
    >
      <>
        <Input
          name="Desk"
          onChange={(e) => {
            setNewDesk({
              ...newDesk,
              deskName: e.target.value ?? "",
            });
          }}
          value={newDesk?.deskName}
          placeholder="Desk Name"
        />
        <>
          <div>Desk Type</div>
          <FilterListbox
            key={"deskTypeListBox"}
            items={data.deskTypes}
            selectedItem={selectedDeskType}
            setSelectedItem={(o) => setSelectedDeskType(o)}
            getName={(deskType) =>
              deskType ? deskType.deskTypeName : "No type selected"
            }
          />
        </>

        <div>Location</div>
        <FilterListbox
          key={"locationListBox"}
          items={data.locations}
          selectedItem={selectedLocation}
          setSelectedItem={(o) => setSelectedLocation(o)}
          getName={(location) =>
            location ? location.locationName : "select location"
          }
        />

        <div>Building</div>
        <FilterListbox
          key={"buildingListBox"}
          items={data.buildings}
          selectedItem={selectedBuilding}
          setSelectedItem={(o) => setSelectedBuilding(o)}
          getName={(building) =>
            building ? building.buildingName : "select building"
          }
        />

        <div>Floor</div>
        <FilterListbox
          key={"floorListBox"}
          items={data.floors}
          selectedItem={selectedFloor}
          setSelectedItem={(o) => setSelectedFloor(o)}
          getName={(floor) => (floor ? floor.floorName : "select floor")}
        />

        <div>Room</div>
        <FilterListbox
          key={"roomListBox"}
          items={data.rooms}
          selectedItem={room}
          setSelectedItem={(o) => setRoom(o)}
          getName={(room) => (room ? room.roomName : "select room")}
        />
      </>
    </GenericAddResourceModal>
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
    const buildings = await (
      await getBuildings(session)
    ).filter((b) => !b.isMarkedForDeletion);
    const locations: ILocation[] = buildings.map((b) => ({
      locationName: b.location,
    }));
    const floors = (await getFloors(session)).filter(
      (b) => !b.isMarkedForDeletion
    );
    const rooms = (await getRooms(session)).filter(
      (b) => !b.isMarkedForDeletion
    );
    const desks = (await getDesks(session)).filter(
      (b) => !b.isMarkedForDeletion
    );
    const deskTypes = (await getDeskTypes(session)).filter(
      (b) => !b.isMarkedForDeletion
    );
    return {
      props: {
        data: {
          locations,
          buildings,
          floors,
          rooms,
          desks,
          deskTypes,
        },
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
