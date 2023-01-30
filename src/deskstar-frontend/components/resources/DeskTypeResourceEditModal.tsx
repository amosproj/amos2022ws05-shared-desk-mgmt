import { Session } from "next-auth";
import React, { useState } from "react";
import { toast } from "react-toastify";
import { getFloors, getRooms } from "../../lib/api/ResourceService";
import { IBuilding } from "../../types/building";
import { IDeskType } from "../../types/desktypes";
import { IFloor } from "../../types/floor";
import { ILocation } from "../../types/location";
import { IRoom } from "../../types/room";
import FilterListbox from "../FilterListbox";
import Input from "../forms/Input";

const DeskTypeResourceEditModal = ({
    desktype,
    isOpen,
    setState,
    session,
    updateDeskType,
    deskTypeName,
    setDeskTypeName,
}: {
    desktype: IDeskType | undefined;
    isOpen: boolean;
    setState: Function;
    session: Session;
    updateDeskType: Function;
    deskTypeName: string;
    setDeskTypeName: Function;
}) => {
    if (!desktype) return (<></>)

    return (
        <>
            <div className={isOpen ? "modal modal-open " : "modal"}>
                <div className="modal-box">
                    <a className="btn btn-sm btn-circle float-right" onClick={() => setState(false)}>
                        x
                    </a>
                    <h1 className="text-2xl pb-4">Edit Desk Type</h1>
                    <Input
                        name="Change Desk Type Name"
                        onChange={(e) => {
                            setDeskTypeName(e.target.value);
                            console.log(e.target.value);
                        }}
                        value={deskTypeName}
                        placeholder={desktype.deskTypeName}
                    />


                    <div className="flex justify-end">
                        <button
                            className="btn bg-deskstar-green-dark text-black hover:bg-deskstar-green-light"
                            onClick={() => {
                                //check changes
                                if (deskTypeName === '') {
                                    toast.error("Desk type name must not be empty");
                                    return;
                                }
                                const changeFloorName = desktype.deskTypeName === deskTypeName ? undefined : deskTypeName;
                                const deskTypeDto = { deskTypeName: changeFloorName };
                                updateDeskType(deskTypeDto, desktype);
                            }}
                        >Confirm</button>
                    </div>
                </div>
            </div>
        </>
    );
};

export default DeskTypeResourceEditModal;