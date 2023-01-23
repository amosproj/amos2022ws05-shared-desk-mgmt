import React from "react";
import { IDesk } from "../../types/desk";
import { IRoom } from "../../types/room";

const DeskResourceEditModal = ({
    desk,
    rooms
}: {
    desk: IDesk | undefined;
    rooms: IRoom[];
}) => {
    if (!desk) return (<></>)
    
    return (
        <div id={`#edit-desk-modal`} className="modal">
            <div className="modal-box">
                <a href="#close" className="btn btn-sm btn-circle float-right">
                    x
                </a>
                {desk.deskName}

            </div>
        </div>
    );
};

export default DeskResourceEditModal;
