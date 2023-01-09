import { useState } from 'react';
import { Dialog } from '@headlessui/react';
import {IUser} from "../types/users";
import Input from "./forms/Input";

import React from 'react';

export default function EditUserModal({
    user,
    action,
  }: {
    user: IUser;
    action: Function;
  }) {
  let [isOpen, setIsOpen] = useState(true)

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");

  return (
    <Dialog open={isOpen} onClose={() => setIsOpen(false)}>
      <Dialog.Panel>
        <Dialog.Title>Edit {user.firstName} {user.lastName} </Dialog.Title>
        <div>
            <div>
            <Input name="first name" onChange={(e) => { setFirstName(e.target.value) }} value={user.firstName} placeholder="first name" />
            <Input name="last name" onChange={(e) => { setLastName(e.target.value) }} value={user.lastName} placeholder="last name" />
            </div>
            <br/>
            <div>
            <Input name="E-Mail" onChange={(e) => { setEmail(e.target.value) }} value={user.firstName} placeholder="E-Mail" />
                
            </div>
        </div>
        <button className="btn-success" onClick={() => {setIsOpen(false); action(firstName,lastName, email);}}>Save</button>
        <button onClick={() => setIsOpen(false)}>Cancel</button>
      </Dialog.Panel>
    </Dialog>
  )
}