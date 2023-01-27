import { IDesk } from "../types/desk";
import React, { useState } from "react";

const DesksTable = ({
  desks,
  onBook,
}: {
  desks: IDesk[];
  onBook: Function;
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            <th className="w-1/4 bg-deskstar-green-light text-left">Desk</th>
            <th className="bg-deskstar-green-light text-left">Type</th>
            <th className="bg-deskstar-green-light"></th>
          </tr>
        </thead>
        <tbody>
          {desks.map((desk: IDesk) => (
            <DeskTableEntry key={desk.deskId} desk={desk} onBook={onBook} />
          ))}
        </tbody>
      </table>
    </div>
  );
};

const DeskTableEntry = ({
  desk,
  onBook,
}: {
  desk: IDesk;
  onBook: Function;
}) => {
  const [buttonText, setButtonText] = useState("Book");
  return (
    <tr className="hover">
      <td className="text-left font-bold">{desk.deskName}</td>
      <td className="text-left">{desk.deskTyp}</td>
      <td className="text-right">
        <button
          className="btn btn-success"
          onClick={(event) => onBook(event, desk, setButtonText)}
        >
          {buttonText}
        </button>
      </td>
    </tr>
  );
};

export default DesksTable;
