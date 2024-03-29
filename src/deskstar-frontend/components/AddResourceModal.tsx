import { useSession } from "next-auth/react";
import { useState } from "react";
import {
  createBuilding,
  createDesk,
  createDeskType,
  createFloor,
  createRoom,
  getAllDesks,
  getDesks,
  getFloors,
  getRooms,
} from "../lib/api/ResourceService";
import { IBuilding } from "../types/building";
import { IDeskType } from "../types/desktypes";
import { IFloor } from "../types/floor";
import { ILocation } from "../types/location";
import { IRoom } from "../types/room";
import { toast } from "react-toastify";
import FilterListbox from "./FilterListbox";
import Input from "./forms/Input";
import { classes } from "../lib/helpers";
import { IDesk } from "../types/desk";

const AddResourceModal = ({
  buildings: origBuildings,
  deskTypes: origDeskTypes,
  setLocations: setParentLocations,
  setBuildings: setParentBuildings,
  desks,
  setDesks: setParentDesks,
  setDeskTypes: setParentDeskTypes,
  setFloors: setParentFloors,
  setRooms: setParentRooms,
}: {
  buildings: IBuilding[];
  deskTypes: IDeskType[];
  setLocations: (locations: ILocation[]) => void;
  setBuildings: (buildings: IBuilding[]) => void;
  desks: IDesk[];
  setDesks: (desks: IDesk[]) => void;
  setDeskTypes: (deskTypes: IDeskType[]) => void;
  setFloors: (floors: IFloor[]) => void;
  setRooms: (rooms: IRoom[]) => void;
}) => {
  let { data: session } = useSession();

  const resourceTypes: string[] = [
    "Building",
    "Floor",
    "Room",
    "Desk",
    "DeskType",
  ];
  const [selectedResourceType, setSelectedResourceType] = useState("Desk");
  const [isLoading, setIsLoading] = useState(false);

  const [buildingName, setBuildingName] = useState("");
  const [locationName, setLocationName] = useState("");
  const [floorName, setFloorName] = useState("");
  const [roomName, setRoomName] = useState("");
  const [deskName, setDeskName] = useState("");
  const [deskTypeName, setDeskTypeName] = useState("");

  const [deskType, setDeskType] = useState<IDeskType | null>();
  const [room, setRoom] = useState<IRoom | null>();
  const [floor, setFloor] = useState<IFloor | null>();
  const [building, setBuilding] = useState<IBuilding | null>();
  const [location, setLocation] = useState<ILocation | null>();
  const uniqueLocation = (ogBuildings: IBuilding[]) => {
    const t = new Map<string, ILocation>();
    ogBuildings.forEach((element) =>
      t.set(element.location, { locationName: element.location })
    );
    return Array.from(t.values());
  };
  const [locations, setLocations] = useState<ILocation[]>(
    uniqueLocation(origBuildings)
  );

  const [buildings, setBuildings] = useState<IBuilding[]>([]);
  const [floors, setFloors] = useState<IFloor[]>([]);
  const [rooms, setRooms] = useState<IRoom[]>([]);
  const [deskTypes, setDeskTypes] = useState<IDeskType[]>(origDeskTypes);

  async function onSelectedLocationChange(
    selectedLocation: ILocation | null | undefined
  ) {
    if (!selectedLocation) {
      return;
    }

    setLocation(selectedLocation);
    let filteredBuildings = origBuildings.filter(
      (building) => selectedLocation.locationName === building.location
    );

    setBuildings(filteredBuildings);
    setBuilding(null);
    setFloor(null);
    setRoom(null);
  }

  async function onSelectedBuildingChange(
    selectedBuilding: IBuilding | null | undefined
  ) {
    if (!selectedBuilding) return;

    setBuilding(selectedBuilding);

    if (!session) return [];

    try {
      const resFloors = (
        await getFloors(session, selectedBuilding.buildingId)
      ).filter((floor) => !floor.isMarkedForDeletion);
      setFloors(resFloors);
    } catch (error) {
      toast.error(`${error}`);
    }

    setFloor(null);
    setRoom(null);
  }

  async function onSelectedFloorChange(
    selectedFloor: IFloor | null | undefined
  ) {
    if (!selectedFloor) return;

    setFloor(selectedFloor);
    if (!session) return [];

    try {
      const resRooms = await (
        await getRooms(session, selectedFloor.floorId)
      ).filter((room) => !room.isMarkedForDeletion);

      setRooms(resRooms);
    } catch (error) {
      toast.error(`${error}`);
    }

    setRoom(null);
  }

  async function onSelectedDeskTypeChange(
    onSelectedDeskType: IDeskType | null | undefined
  ) {
    if (!onSelectedDeskType) return;
    setDeskType(onSelectedDeskType);
  }

  async function addBuilding() {
    if (!session) return;
    if (!buildingName) {
      toast.warn("please enter a building name");
      return;
    }
    if ((!locationName || locationName === "") && !location) {
      toast.warn("please choose or enter a location");
      return;
    }

    setIsLoading(true);
    let res = await createBuilding(session, {
      buildingName: buildingName,
      location: location ? location.locationName : locationName,
    });
    if (res.message.toLowerCase().includes("success")) {
      origBuildings.push(res.data as IBuilding);
      setBuildings(origBuildings);
      setParentBuildings(origBuildings);
      const tmp = new Map<string, ILocation>();
      [
        ...locations,
        { locationName: (res.data as IBuilding).location },
      ].forEach((element) =>
        tmp.set(element.locationName, { locationName: element.locationName })
      );
      setLocations(Array.from(tmp.values()));
      setParentLocations(Array.from(tmp.values()));
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }

    setIsLoading(false);
  }

  async function addFloor() {
    if (!session) return;
    if (!floorName) {
      toast.warn("please enter a floor name");
      return;
    }
    if (!location) {
      toast.warn("please choose a location");
      return;
    }
    if (!building) {
      toast.warn("please choose a building");
      return;
    }

    setIsLoading(true);
    let res = await createFloor(session, {
      buildingId: building.buildingId,
      floorName: floorName,
    });

    if (res.message.toLowerCase().includes("success")) {
      setFloors([...floors, res.data as IFloor]);
      setParentFloors([...floors, res.data as IFloor]);
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    setIsLoading(false);
  }

  async function addRoom() {
    if (!session) return;
    if (!roomName) {
      toast.warn("please enter a room name");
      return;
    }
    if (!location) {
      toast.warn("please choose a location");
      return;
    }
    if (!building) {
      toast.warn("please choose a building");
      return;
    }
    if (!floor) {
      toast.warn("please choose a floor");
      return;
    }

    setIsLoading(true);
    let res = await createRoom(session, {
      floorId: floor.floorId,
      roomName: roomName,
    });
    if (res.message.toLowerCase().includes("success")) {
      setRooms([...rooms, res.data as IRoom]);
      setParentRooms([...rooms, res.data as IRoom]);
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    setIsLoading(false);
  }

  async function addDeskType() {
    if (!session) return;
    if (!deskTypeName) {
      toast.warn("please enter a desk type name");
      return;
    }

    setIsLoading(true);
    let res = await createDeskType(session, { deskTypeName: deskTypeName });

    if (res.message.toLowerCase().includes("success")) {
      setDeskTypes([...deskTypes, res.data as IDeskType]);
      setParentDeskTypes([...deskTypes, res.data as IDeskType]);
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    setIsLoading(false);
  }

  async function addDesk() {
    if (!session) return;
    if (deskName === "") {
      toast.warn("please enter a desk name");
      return;
    }
    if (!deskType) {
      toast.warn("please choose a desk type");
      return;
    }
    if (!location) {
      toast.warn("please choose a location");
      return;
    }
    if (!building) {
      toast.warn("please choose a building");
      return;
    }
    if (!floor) {
      toast.warn("please choose a floor");
      return;
    }
    if (!room) {
      toast.warn("please choose a room");
      return;
    }

    setIsLoading(true);
    let res = await createDesk(session, {
      deskName: deskName,
      deskTypeId: deskType.deskTypeId,
      roomId: room?.roomId,
    });
    if (res.message.toLowerCase().includes("success")) {
      const desk = (await getDesks(session)).find(
        (d) => d.deskId === (res.data as any).deskId
      );
      if (desk) setParentDesks([...desks, desk]);
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    setIsLoading(false);
  }

  return (
    <>
      <div id="create-resource-modal" className="modal">
        <div className="modal-box text-left">
          <a href="#close" className="btn btn-sm btn-circle float-right">
            x
          </a>
          <h1 className="text-2xl pb-4">Add Resources</h1>

          <div>Resource Type</div>

          <div className="flex items-center">
            {resourceTypes.map((type: string) => (
              <button
                key={type}
                className={`btn mr-2 ${
                  selectedResourceType === type
                    ? "bg-primary text-black hover:bg-secondary"
                    : ""
                }`}
                onClick={() => {
                  if (selectedResourceType !== type) {
                    setBuildingName("");
                    setFloorName("");
                    setLocationName("");
                    setRoomName("");
                    setDeskName("");
                    setDeskTypeName("");
                  }
                  setSelectedResourceType(type);
                }}
              >
                {type}
              </button>
            ))}
          </div>

          {selectedResourceType === "Building" && (
            <>
              <Input
                name="Building"
                onChange={(e) => {
                  setBuildingName(e.target.value);
                }}
                value={buildingName}
                placeholder="Building Name"
              />
              <Input
                name="Location"
                onChange={(e) => {
                  setLocationName(e.target.value);
                  setLocation(null);
                }}
                value={locationName}
                placeholder="New Location"
              />

              <div>Existing Location</div>
              <FilterListbox
                key={"locationListBox"}
                items={locations}
                selectedItem={location}
                setSelectedItem={(o) => {
                  onSelectedLocationChange(o);
                  setLocationName("");
                }}
                getName={(location) =>
                  location ? location.locationName : "select location"
                }
              />

              <a
                className="btn text-black bg-primary hover:bg-secondary border-primary hover:border-secondary float-right"
                onClick={() => addBuilding()}
              >
                Confirm
              </a>
            </>
          )}
          {selectedResourceType === "Floor" && (
            <>
              <Input
                name="Floor"
                onChange={(e) => {
                  setFloorName(e.target.value);
                }}
                value={floorName}
                placeholder="Floor Name"
              />

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
              <a
                className="btn text-black bg-primary hover:bg-secondary border-primary hover:border-secondary float-right"
                onClick={() => addFloor()}
              >
                Confirm
              </a>
            </>
          )}
          {selectedResourceType === "Room" && (
            <>
              <Input
                name="Room"
                onChange={(e) => {
                  setRoomName(e.target.value);
                }}
                value={roomName}
                placeholder="Room Name"
              />

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
                    getName={(floor) =>
                      floor ? floor.floorName : "select floor"
                    }
                  />
                </>
              )}
              <a
                className="btn text-black bg-primary hover:bg-secondary border-primary hover:border-secondary float-right"
                onClick={() => addRoom()}
              >
                Confirm
              </a>
            </>
          )}

          {selectedResourceType === "Desk" && (
            <>
              <Input
                name="Desk"
                onChange={(e) => {
                  setDeskName(e.target.value);
                }}
                value={deskName}
                placeholder="Desk Name"
              />
              <>
                <div>Desk Type</div>
                <FilterListbox
                  key={"deskTypeListBox"}
                  items={deskTypes}
                  selectedItem={deskType}
                  setSelectedItem={(o) => onSelectedDeskTypeChange(o)}
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
                    getName={(floor) =>
                      floor ? floor.floorName : "select floor"
                    }
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
              <a
                className="btn text-black bg-primary hover:bg-secondary border-primary hover:border-secondary float-right"
                onClick={() => addDesk()}
              >
                Confirm
              </a>
            </>
          )}

          {selectedResourceType === "DeskType" && (
            <>
              <Input
                name="Desk Type"
                onChange={(e) => {
                  setDeskTypeName(e.target.value);
                }}
                value={deskTypeName}
                placeholder="Desk Type Name"
              />
              <a
                className={classes(
                  isLoading ? "disabled" : "",
                  "btn text-black bg-primary hover:bg-secondary border-primary hover:border-secondary float-right"
                )}
                onClick={() => addDeskType()}
              >
                Confirm
              </a>
            </>
          )}
          {isLoading && (
            <div className="h-2">
              <progress className="progress"></progress>
            </div>
          )}
          {!isLoading && <div className="h-2 mb-4" />}
        </div>
      </div>
    </>
  );
};

export default AddResourceModal;
