import { IDesk } from "../../types/desk";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import React from "react";

const DeskResourceTable = ({
  desks,
  onEdit,
  onDelete,
}: {
  desks: IDesk[];
  onEdit: Function;
  onDelete: Function;
}) => {
  return (
    <div className="overflow-x-auto table-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-deskstar-green-light text-left">Desk</th>
            <th className="bg-deskstar-green-light text-left">Room</th>
            <th className="bg-deskstar-green-light text-left">Floor</th>
            <th className="bg-deskstar-green-light text-left">Building</th>
            <th className="bg-deskstar-green-light text-left">Location</th>
            <th className="bg-deskstar-green-light"></th>
          </tr>
        </thead>
        <tbody>
          {desks.map((desk: IDesk) => (
            <DeskResourceTableEntry
              key={desk.deskId}
              desk={desk}
              onEdit={onEdit}
              onDelete={onDelete}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
};

const DeskResourceTableEntry = ({
  desk,
  onEdit,
  onDelete,
}: {
  desk: IDesk;
  onEdit: Function;
  onDelete: Function;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{desk.deskName}</td>
      <td className="text-left">{desk.roomName}</td>
      <td className="text-left">{desk.floorName}</td>
      <td className="text-left">{desk.buildingName}</td>
      <td className="text-left">{desk.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 text-right">
          {onDelete && (
            <button className="btn btn-ghost" onClick={() => onDelete(desk)}>
              <FaTrashAlt color="red" />
            </button>
          )}
          {onEdit && (
            <button className="btn btn-ghost" onClick={() => onEdit(desk)}>
              <FaEdit />
            </button>
          )}
        </td>
      )}
    </tr>
  );
};

export default DeskResourceTable;
