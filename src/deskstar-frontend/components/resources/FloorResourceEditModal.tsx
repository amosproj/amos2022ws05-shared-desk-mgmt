import { Session } from "next-auth";
import React, { useState } from "react";
import { toast } from "react-toastify";
import { IBuilding } from "../../types/building";
import { ILocation } from "../../types/location";
import { IFloor } from "../../types/floor";
import Input from "../forms/Input";
import FilterListbox from "../FilterListbox";

const FloorResourceEditModal = ({
  floor,
  origBuildings,
  isOpen,
  setState,
  updateFloor,
  floorName,
  setFloorName,
  session,
}: {
  floor: IFloor | undefined;
  origBuildings: IBuilding[];
  isOpen: boolean;
  setState: Function;
  updateFloor: Function;
  session: Session;
  floorName: string;
  setFloorName: Function;
}) => {
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
  const [building, setBuilding] = useState<IBuilding | null>();
  const [location, setLocation] = useState<ILocation | null>();
  const [buildings, setBuildings] = useState<IBuilding[]>([]);

  if (!floor) return <></>;

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
  }

  async function onSelectedBuildingChange(
    selectedBuilding: IBuilding | null | undefined
  ) {
    if (!selectedBuilding) {
      return;
    }

    setBuilding(selectedBuilding);
    if (!session) {
      return [];
    }
  }
  async function clearState() {
    setBuilding(null);
    setLocation(null);
  }
  return (
    <>
      <div className={isOpen ? "modal modal-open overflow-y-auto" : "modal"}>
        <div className="modal-box overflow-y-visible">
          <a
            className="btn btn-sm btn-circle float-right"
            onClick={() => {
              clearState();
              setState(false);
            }}
          >
            x
          </a>
          <h1 className="text-2xl pb-4">Edit Floor</h1>
          <Input
            name="Change Floor Name"
            onChange={(e) => {
              setFloorName(e.target.value);
              console.log(e.target.value);
            }}
            value={floorName}
            placeholder={floor.floorName}
          />
          <h6 className="text-lg pt-1">Select New Building</h6>
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
          <div className="flex justify-end">
            <button
              className="btn bg-deskstar-green-dark text-black hover:bg-deskstar-green-light"
              onClick={() => {
                //check changes
                if (floorName === "") {
                  toast.error("Floor name must not be empty");
                  return;
                }
                const changeFloorName =
                  floor.floorName === floorName ? undefined : floorName;
                const selectedBuilding =
                  building == undefined ? undefined : building;
                const updateBuildingDto = {
                  floorName: changeFloorName,
                  buildingId: selectedBuilding?.buildingId,
                };
                updateFloor(updateBuildingDto, floor);
                clearState();
              }}
            >
              Confirm
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default FloorResourceEditModal;
