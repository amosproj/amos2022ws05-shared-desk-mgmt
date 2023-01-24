import { FaTrashAlt, FaEdit } from "react-icons/fa";
import React from "react";
import { IFloor } from "../../types/floor";

const FloorResourceTable = ({
  floors,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  floors: IFloor[];
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (floor: IFloor) => Promise<void>;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-deskstar-green-light text-left">Floor</th>
            <th className="bg-deskstar-green-light text-left">Building</th>
            <th className="bg-deskstar-green-light text-left">Location</th>
            <th className="bg-deskstar-green-light"></th>
          </tr>
        </thead>
        <tbody>
          {floors.map((floor: IFloor) => (
            <FloorResourceTableEntry
              key={floor.floorId}
              floor={floor}
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

const FloorResourceTableEntry = ({
  floor,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  floor: IFloor;
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (floor: IFloor) => Promise<void>;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{floor.floorName}</td>
      <td className="text-left">{floor.buildingName}</td>
      <td className="text-left">{floor.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 text-right">
          {onDelete && (
            <button className="btn btn-ghost" onClick={() => onDelete(floor)}>
              <FaTrashAlt color="red" />
            </button>
          )}
          {onEdit && (
            <button className="btn btn-ghost" onClick={() => onEdit(floor)}>
              <FaEdit />
            </button>
          )}
          {onRestoreUpdate && (
            <th className="bg-deskstar-green-light text-center">Restore</th>
          )}
        </td>
      )}
    </tr>
  );
};

export default FloorResourceTable;
