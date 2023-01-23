import React from "react";
import { IBuilding } from "../../types/building";
import { ILocation } from "../../types/location";

const BuildingResourceEditModal = ({
    building,
    locations
}: {
    building:IBuilding;
    locations:ILocation[];
}) => {
    return (
        <>
            <input type="checkbox" id={building.buildingId} className="modal-toggle" />
            <div className="modal">
                <div className="modal-box">
                    {building.buildingName}
                </div>
            </div>
        </>
    );
};

export default BuildingResourceEditModal;