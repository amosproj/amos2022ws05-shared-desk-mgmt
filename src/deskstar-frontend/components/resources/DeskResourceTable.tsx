import { IDesk } from "../../types/desk";
import { FaTrashAlt, FaPencilAlt, FaTrashRestore } from "react-icons/fa";
import React from "react";
import DeskResourceEditModal from "./DeskResourceEditModal";

const DeskResourceTable = ({
  desks,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  desks: IDesk[];
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (desk: IDesk) => Promise<void>;
}) => {
  return (
    <div className="overflow-x-auto table-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-secondary text-left">Desk</th>
            <th className="bg-secondary text-left">Desk Type</th>
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
          {desks.map((desk: IDesk) => (
            <DeskResourceTableEntry
              key={desk.deskId}
              desk={desk}
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

const DeskResourceTableEntry = ({
  desk,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  desk: IDesk;
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (desk: IDesk) => Promise<void>;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{desk.deskName}</td>
      <td className="text-left">{desk.deskTyp}</td>
      <td className="text-left">{desk.roomName}</td>
      <td className="text-left">{desk.floorName}</td>
      <td className="text-left">{desk.buildingName}</td>
      <td className="text-left">{desk.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 pr-2 text-right">
          {onEdit && (
            <button
              className="btn btn-ghost"
              onClick={() => {
                onEdit(desk);
              }}
            >
              <FaPencilAlt />
            </button>
          )}
          {onDelete && (
            <button className="btn btn-ghost" onClick={() => onDelete(desk)}>
              <FaTrashAlt color="red" />
            </button>
          )}
        </td>
      )}
      {onRestoreUpdate && (
        <td className="text-right">
          <button
            className="btn btn-ghost"
            onClick={() => onRestoreUpdate(desk)}
          >
            <FaTrashRestore color="green" />
          </button>
        </td>
      )}
    </tr>
  );
};

export default DeskResourceTable;
