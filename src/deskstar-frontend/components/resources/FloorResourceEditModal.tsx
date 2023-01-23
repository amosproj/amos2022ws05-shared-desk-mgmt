import React from "react";
import { IBuilding } from "../../types/building";
import { IFloor } from "../../types/floor";

const FloorResourceEditModal = ({
    floor,
    buildings
}: {
    floor:IFloor;
    buildings:IBuilding;
}) => {
    return (
        <>
            <input type="checkbox" id={floor.floorId} className="modal-toggle" />
            <div className="modal">
                <div className="modal-box">
                    {floor.floorName}
                </div>
            </div>
        </>
    );
};

export default FloorResourceEditModal;