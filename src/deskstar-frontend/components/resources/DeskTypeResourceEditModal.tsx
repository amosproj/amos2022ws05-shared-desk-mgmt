import React from "react";
import { IDeskType } from "../../types/desktypes";

const DeskTypeResourceEditModal = ({
    desktype,
}: {
    desktype:IDeskType
}) => {
    return (
        <>
            <input type="checkbox" id={desktype.deskTypeId} className="modal-toggle" />
            <div className="modal">
                <div className="modal-box">
                    {desktype.deskTypeName}
                </div>
            </div>
        </>
    );
};

export default DeskTypeResourceEditModal;