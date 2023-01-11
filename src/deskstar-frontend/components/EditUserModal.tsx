import { useState } from "react";
import { Dialog } from "@headlessui/react";
import { IUser } from "../types/users";
import Input from "./forms/Input";
import React from "react";

export default function EditUserModal({
  user,
  action,
  isOpen,
  setIsOpen,
}: {
  user?: IUser;
  action: Function;
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
}) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");

  return (
    <Dialog open={isOpen} onClose={() => setIsOpen(false)}>
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4 ">
        <Dialog.Panel
          as="div"
          className={"card bg-white dark:bg-gray-300 dark:text-black w-96"}
        >
          <div className="card-body">
            <Dialog.Title as="div" className="card-title flex justify-between">
              <div>
                Edit {user?.firstName} {user?.lastName}{" "}
              </div>
            </Dialog.Title>
            <Dialog.Description as="div" className={"py-4"}>
              <div>
                <div>
                  <Input
                    name="first name"
                    onChange={(e) => {
                      setFirstName(e.target.value);
                    }}
                    placeholder={user?.firstName}
                    value={firstName}
                  />
                  <Input
                    name="last name"
                    onChange={(e) => {
                      setLastName(e.target.value);
                    }}
                    value={lastName}
                    placeholder={user?.lastName}
                  />
                </div>
                <div>
                  <Input
                    name="E-Mail"
                    onChange={(e) => {
                      setEmail(e.target.value);
                    }}
                    value={email}
                    placeholder={user?.email}
                  />
                </div>
              </div>
              <div className="card-actions justify-end mt-4">
                <button
                  className="btn btn-success"
                  onClick={() => {
                    setIsOpen(false);
                    if (user != null) {
                      user.firstName = firstName;
                      user.lastName = lastName;
                      user.email = email;
                      action(user);
                    }
                  }}
                >
                  Save
                </button>
                <button className="btn" onClick={() => setIsOpen(false)}>
                  Cancel
                </button>
              </div>
            </Dialog.Description>
          </div>
        </Dialog.Panel>
      </div>
    </Dialog>
  );
}
