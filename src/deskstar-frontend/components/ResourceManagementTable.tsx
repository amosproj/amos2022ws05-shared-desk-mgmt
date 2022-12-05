import { IDesk } from "../types/desk";
import { FaTrashAlt, FaEdit } from "react-icons/fa";
import React from "react";

const ResourceManagementTable = ({ desks, onEdit, onDelete }: { desks: IDesk[], onEdit: Function, onDelete: Function }) => {
    return (
        <div className="overflow-x-auto">
            <table className="table table-zebra w-full">
                <thead className="dark:text-black">
                    <tr>
                        {/* set size of Desk column */}
                        <th className="bg-deskstar-green-light text-left">Desk</th>
                        <th className="bg-deskstar-green-light text-left">Location</th>
                        <th className="bg-deskstar-green-light text-left">Building</th>
                        <th className="bg-deskstar-green-light text-left">Room</th>
                        <th className="bg-deskstar-green-light text-left">Floor</th>
                        <th className="bg-deskstar-green-light"></th>
                        <th className="bg-deskstar-green-light"></th>
                    </tr>
                </thead>
                <tbody>
                    {desks.map((desk: IDesk) => (
                        <ResourceManagementTableEntry key={desk.deskId} desk={desk} onEdit={onEdit} onDelete={onDelete} />
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const ResourceManagementTableEntry = ({ desk, onEdit, onDelete }: { desk: IDesk, onEdit: Function, onDelete: Function }) => {
    return (
        <tr className="hover">
            <td className="text-left font-bold">{desk.deskName}</td>
            <td className="text-left">{desk.location}</td>
            <td className="text-left">{desk.building}</td>
            <td className="text-left">{desk.room}</td>
            <td className="text-left">{desk.floor}</td>
            {onEdit && (
                <td className="p-0">
                    <button className="btn btn-ghost" onClick={() => onEdit(desk)}>
                        <FaEdit />
                    </button>
                </td>
            )}
            {onDelete && (
                <td className="p-0">
                    <button className="btn btn-ghost" onClick={() => onDelete(desk)}>
                        <FaTrashAlt color="red" />
                    </button>
                </td>
            )}
        </tr>
    );
};

export default ResourceManagementTable;
