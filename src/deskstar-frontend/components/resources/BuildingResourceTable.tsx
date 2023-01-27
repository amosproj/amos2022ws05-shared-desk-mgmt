import { IDesk } from "../../types/desk";
import { FaTrashAlt, FaEdit, FaTrashRestore } from "react-icons/fa";
import React from "react";
import { IBuilding } from "../../types/building";

const BuildingResourceTable = ({
  buildings,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  buildings: IBuilding[];
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (building: IBuilding) => Promise<void>;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-deskstar-green-light text-left">Building</th>
            <th className="bg-deskstar-green-light text-left">Location</th>
            {(onDelete || onEdit) && (
              <th className="bg-deskstar-green-light"></th>
            )}
            {onRestoreUpdate && (
              <th className="bg-deskstar-green-light text-left">Restore</th>
            )}
          </tr>
        </thead>
        <tbody>
          {buildings.map((building: IBuilding) => (
            <BuildingResourceTableEntry
              key={building.buildingId}
              building={building}
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

const BuildingResourceTableEntry = ({
  building,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  building: IBuilding;
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (building: IBuilding) => Promise<void>;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{building.buildingName}</td>
      <td className="text-left">{building.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 text-right">
          {onDelete && (
            <button
              className="btn btn-ghost"
              onClick={() => onDelete(building)}
            >
              <FaTrashAlt color="red" />
            </button>
          )}
          {onEdit && (
            <button className="btn btn-ghost" onClick={() => onEdit(building)}>
              <FaEdit />
            </button>
          )}
        </td>
      )}
      {onRestoreUpdate && (
        <td className="text-right">
          <button
            className="btn btn-ghost"
            onClick={() => onRestoreUpdate(building)}
          >
            <FaTrashRestore color="green" />
          </button>
        </td>
      )}
    </tr>
  );
};

export default BuildingResourceTable;
