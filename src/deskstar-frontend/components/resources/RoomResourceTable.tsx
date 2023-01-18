import { FaTrashAlt, FaEdit } from "react-icons/fa";
import React from "react";
import { IRoom } from "../../types/room";

const RoomResourceTable = ({
  rooms,
  onEdit,
  onDelete,
}: {
  rooms: IRoom[];
  onEdit: Function;
  onDelete: Function;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-deskstar-green-light text-left">Room</th>
            <th className="bg-deskstar-green-light text-left">Floor</th>
            <th className="bg-deskstar-green-light text-left">Building</th>
            <th className="bg-deskstar-green-light text-left">Location</th>
            <th className="bg-deskstar-green-light"></th>
          </tr>
        </thead>
        <tbody>
          {rooms.map((room: IRoom) => (
            <RoomResourceTableEntry
              key={room.roomId}
              room={room}
              onEdit={onEdit}
              onDelete={onDelete}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
};

const RoomResourceTableEntry = ({
  room,
  onEdit,
  onDelete,
}: {
  room: IRoom;
  onEdit: Function;
  onDelete: Function;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{room.roomName}</td>
      <td className="text-left">{room.floor}</td>
      <td className="text-left">{room.building}</td>
      <td className="text-left">{room.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 text-right">
          {onDelete && (
            <button className="btn btn-ghost" onClick={() => onDelete(room)}>
              <FaTrashAlt color="red" />
            </button>
          )}
          {onEdit && (
            <button className="btn btn-ghost" onClick={() => onEdit(room)}>
              <FaEdit />
            </button>
          )}
        </td>
      )}
    </tr>
  );
};

export default RoomResourceTable;
