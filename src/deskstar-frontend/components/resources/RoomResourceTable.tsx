import { FaTrashAlt, FaPencilAlt, FaTrashRestore } from "react-icons/fa";
import React from "react";
import { IRoom } from "../../types/room";

const RoomResourceTable = ({
  rooms,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  rooms: IRoom[];
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (room: IRoom) => Promise<void>;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-secondary text-left">Room</th>
            <th className="bg-secondary text-left">Floor</th>
            <th className="bg-secondary text-left">Building</th>
            <th className="bg-secondary text-left">Location</th>
            {(onDelete || onEdit) && <th className="bg-secondary"></th>}
            {onRestoreUpdate && (
              <th className="bg-secondary text-right">Restore</th>
            )}
          </tr>
        </thead>
        <tbody>
          {rooms.map((room: IRoom) => (
            <RoomResourceTableEntry
              key={room.roomId}
              room={room}
              onEdit={onEdit}
              onDelete={onDelete}
              onRestoreUpdate={onRestoreUpdate}
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
  onRestoreUpdate,
}: {
  room: IRoom;
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (room: IRoom) => Promise<void>;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{room.roomName}</td>
      <td className="text-left">{room.floor}</td>
      <td className="text-left">{room.building}</td>
      <td className="text-left">{room.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 pr-2 text-right">
          {onEdit && (
            <button className="btn btn-ghost" onClick={() => onEdit(room)}>
              <FaPencilAlt />
            </button>
          )}
          {onDelete && (
            <button className="btn btn-ghost" onClick={() => onDelete(room)}>
              <FaTrashAlt color="red" />
            </button>
          )}
        </td>
      )}
      {onRestoreUpdate && (
        <td className="text-right">
          <button
            className="btn btn-ghost"
            onClick={() => onRestoreUpdate(room)}
          >
            <FaTrashRestore color="green" />
          </button>
        </td>
      )}
    </tr>
  );
};

export default RoomResourceTable;
