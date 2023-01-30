import { Session } from "next-auth";
import { useSession } from "next-auth/react";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { updateBuilding } from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { ILocation } from "../../types/location";
import Input from "../forms/Input";

const BuildingResourceEditModal = ({
    building,
    isOpen,
    setState,
    updateBuilding,
    buildingName,
    setBuildingName,
    location,
    setLocation
}: {
    building: IBuilding | undefined;
    isOpen: boolean;
    setState: Function;
    updateBuilding: Function;
    setBuildingName: Function;
    setLocation: Function;
    buildingName: string;
    location: string;
}) => {
    if (!building) return (<></>)
    return (
        <>
            <div className={isOpen ? "modal modal-open" : "modal"}>
                <div className="modal-box">
                    <a className="btn btn-sm btn-circle float-right" onClick={() => { setState(false); }}>
                        x
                    </a>
                    <h1 className="text-2xl pb-4">Edit Building</h1>
                    <Input
                        name="Change Building Name"
                        onChange={(e) => {
                            setBuildingName(e.target.value);
                            console.log(e.target.value);
                        }}
                        value={buildingName}
                        placeholder={building.buildingName}
                    />
                    <Input
                        name="Change Location"
                        onChange={(e) => {
                            setLocation(e.target.value);
                            console.log(e.target.value);
                        }}
                        value={location}
                        placeholder={building.location}
                    />
                    <div className="flex justify-end">
                        <button
                            className="btn bg-deskstar-green-dark text-black hover:bg-deskstar-green-light"
                            onClick={() => {
                                //check changes
                                if (buildingName === '') {
                                    toast.error("Building name must not be empty");
                                    return;
                                }
                                const changeBuildingName = building.buildingName === buildingName ? undefined : buildingName;
                                const changeLocation = building.location === location ? undefined : location;
                                const updateBuildingDto = { buildingName: changeBuildingName, location: changeLocation, };
                                updateBuilding(updateBuildingDto, building);
                            }}
                        >Confirm</button>
                    </div>
                </div>
            </div>
        </>
    );
};

export default BuildingResourceEditModal;