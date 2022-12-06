import { IDesk } from "../types/desk";
import React from "react";

const DesksTable = ({ desks }: { desks: IDesk[] }) => {
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
            <DeskTableEntry key={desk.deskId} desk={desk} />
          ))}
        </tbody>
      </table>
    </div>
  );
};

const DeskTableEntry = ({ desk }: { desk: IDesk }) => {
  return (
    <tr className="hover">
      <td className="text-left font-bold">{desk.deskName}</td>
      <td className="text-left">{desk.deskTyp}</td>
      <td className="text-right">
        <a href="#book-modal" className="btn btn-success">
          Book
        </a>
        <div id="book-modal" className="modal">
          <div className="modal-box text-left">
            <a href="#close" className="btn btn-sm btn-circle float-right">
              x
            </a>
            <p>
              <b>Desk:</b> {desk.deskName}
            </p>
            <p>
              <b>Type:</b> {desk.deskTyp}
            </p>
            <p>
              <b>Building:</b> {desk.buildingName}
            </p>
            <p>
              <b>Room:</b> {desk.roomName}
            </p>
            <div className="form-group">
              <label className="form-label" htmlFor="start-date">
                <b>Start Date:</b> &nbsp;
              </label>
              <input
                className="form-input"
                type="date"
                id="start-date"
                placeholder="Start Date"
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="start-time">
                <b>Start Time:</b> &nbsp;
              </label>
              <input
                className="form-input"
                type="time"
                id="start-time"
                placeholder="Start Time"
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="end-date">
                <b>End Date:</b> &nbsp;
              </label>
              <input
                className="form-input"
                type="date"
                id="end-date"
                placeholder="End Date"
              />
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="end-time">
                <b>End Time:</b> &nbsp;
              </label>
              <input
                className="form-input"
                type="time"
                id="end-time"
                placeholder="End Time"
              />
            </div>
            <a href="#close" className="btn btn-success float-right">
              Book
            </a>
          </div>
        </div>
      </td>
    </tr>
  );
};

export default DesksTable;
