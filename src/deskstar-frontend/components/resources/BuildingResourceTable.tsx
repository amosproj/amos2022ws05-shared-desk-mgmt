import { IDesk } from "../../types/desk";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import React from "react";
import { IBuilding } from "../../types/building";

const BuildingResourceTable = ({
  buildings,
  onEdit,
  onDelete,
}: {
  buildings: IBuilding[];
  onEdit: Function;
  onDelete: Function;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-deskstar-green-light text-left">Building</th>
            <th className="bg-deskstar-green-light text-left">Location</th>
            <th className="bg-deskstar-green-light"></th>
          </tr>
        </thead>
        <tbody>
          {buildings.map((building: IBuilding) => (
            <BuildingResourceTableEntry
              key={building.buildingId}
              building={building}
              onEdit={onEdit}
              onDelete={onDelete}
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
}: {
  building: IBuilding;
  onEdit: Function;
  onDelete: Function;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{building.buildingName}</td>
      <td className="text-left">{building.location}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 text-right">
          {
            onDelete && <button className="btn btn-ghost" onClick={() => onDelete(building)}>
              <FaTrashAlt color="red" />
            </button>}
          {
            onEdit && <button className="btn btn-ghost" onClick={() => onEdit(building)}>
              <FaEdit />
            </button>
          }
        </td>

      )}
    </tr>
  );
};

export default BuildingResourceTable;
