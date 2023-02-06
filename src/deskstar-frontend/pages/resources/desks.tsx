import { useState } from "react";
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
} from "../../types";
import { CreateDeskDto } from "../../types/models/CreateDeskDto";

type DesksProps = {
  locations: ILocation[];
  buildings: IBuilding[];
  floors: IFloor[];
  rooms: IRoom[];
  deskTypes: IDeskType[];
};

export default function Desks({
  locations,
  buildings,
  floors,
  rooms,
  deskTypes,
}: DesksProps) {
  const [desks, setDesks] = useState([]);

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

      {/* Table */}

      {/* Modals, will only be displayed if opened */}
      <AddDeskModal
        locations={locations}
        buildings={buildings}
        floors={floors}
        rooms={rooms}
        deskTypes={deskTypes}
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
  deskTypes,
}: AddDeskModalProps) {
  const [newDesk, setNewDesk] = useState<CreateDeskDto>({
    deskName: "",
    deskTypeId: "",
    roomId: "",
  });

  const [selectedDeskType, setSelectedDeskType] = useState<IDeskType | null>(
    null
  );

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
            items={deskTypes}
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
          items={locations}
          selectedItem={location}
          setSelectedItem={(o) => onSelectedLocationChange(o)}
          getName={(location) =>
            location ? location.locationName : "select location"
          }
        />

        {location && (
          <>
            <div>Building</div>
            <FilterListbox
              key={"buildingListBox"}
              items={buildings}
              selectedItem={building}
              setSelectedItem={(o) => onSelectedBuildingChange(o)}
              getName={(building) =>
                building ? building.buildingName : "select building"
              }
            />
          </>
        )}

        {building && (
          <>
            <div>Floor</div>
            <FilterListbox
              key={"floorListBox"}
              items={floors}
              selectedItem={floor}
              setSelectedItem={(o) => onSelectedFloorChange(o)}
              getName={(floor) => (floor ? floor.floorName : "select floor")}
            />
          </>
        )}

        {floor && (
          <>
            <div>Room</div>
            <FilterListbox
              key={"roomListBox"}
              items={rooms}
              selectedItem={room}
              setSelectedItem={(o) => setRoom(o)}
              getName={(room) => (room ? room.roomName : "select room")}
            />
          </>
        )}
      </>
    </GenericAddResourceModal>
  );
}
