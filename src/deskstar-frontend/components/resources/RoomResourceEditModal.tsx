import React from "react";
import { IFloor } from "../../types/floor";
import { IRoom } from "../../types/room";

const RoomResourceEditModal = ({
    room,
    floors,
}: {
    room:IRoom;
    floors:IFloor[];
}) => {
    return (
        <>
            <input type="checkbox" id={room.roomId} className="modal-toggle" />
            <div className="modal">
                <div className="modal-box">
                    {room.roomName}
                </div>
            </div>
        </>
    );
};

export default RoomResourceEditModal;