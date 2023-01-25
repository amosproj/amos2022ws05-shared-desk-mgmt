import { FaTrashAlt, FaEdit, FaTrashRestore } from "react-icons/fa";
import React from "react";
import { IDeskType } from "../../types/desktypes";

const DeskTypeResourceTable = ({
  deskTypes: deskTypes,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  deskTypes: IDeskType[];
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (desktype: IDeskType) => Promise<void>;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {/* set size of Desk column */}
            <th className="bg-deskstar-green-light text-left">Desk Type</th>
            {(onDelete || onEdit) && (
              <th className="bg-deskstar-green-light"></th>
            )}
            {onRestoreUpdate && (
              <th className="bg-deskstar-green-light text-left">Restore</th>
            )}
          </tr>
        </thead>
        <tbody>
          {deskTypes.map((deskType: IDeskType) => (
            <DeksTypeTableEntry
              key={deskType.deskTypeId}
              deskType={deskType}
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

const DeksTypeTableEntry = ({
  deskType,
  onEdit,
  onDelete,
  onRestoreUpdate,
}: {
  deskType: IDeskType;
  onEdit?: Function;
  onDelete?: Function;
  onRestoreUpdate?: (desktype: IDeskType) => Promise<void>;
}) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{deskType.deskTypeName}</td>
      {(onDelete || onEdit) && (
        <td className="p-0 text-right">
          {onDelete && (
            <button
              className="btn btn-ghost"
              onClick={() => onDelete(deskType)}
            >
              <FaTrashAlt color="red" />
            </button>
          )}
          {onEdit && (
            <button className="btn btn-ghost" onClick={() => onEdit(deskType)}>
              <FaEdit />
            </button>
          )}
        </td>
      )}
      {onRestoreUpdate && (
        <td className="text-left">
          <button
            className="btn btn-ghost"
            onClick={() => onRestoreUpdate(deskType)}
          >
            <FaTrashRestore color="green" />
          </button>
        </td>
      )}
    </tr>
  );
};

export default DeskTypeResourceTable;
