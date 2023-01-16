import { useEffect, useState } from "react";
import { Dialog } from "@headlessui/react";
import { IUser } from "../types/users";
import Input from "./forms/Input";
import React from "react";

export default function EditUserModal({
  user,
  setUser,
  isOpen,
  setIsOpen,
}: {
  user?: IUser;
  setUser: (newUser: IUser) => Promise<void>;
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
}) {
  const [isLoading, setIsLoading] = useState(false);
  const [firstName, setFirstName] = useState(user?.firstName);
  const [lastName, setLastName] = useState(user?.lastName);
  const [email, setEmail] = useState(user?.email);

  useEffect(() => {
    setFirstName(user?.firstName);
    setLastName(user?.lastName);
    setEmail(user?.email);
  }, [user]);

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
                    value={firstName ?? ""}
                  />
                  <Input
                    name="last name"
                    onChange={(e) => {
                      setLastName(e.target.value);
                    }}
                    value={lastName ?? ""}
                    placeholder={user?.lastName}
                  />
                </div>
                <div>
                  <Input
                    name="E-Mail"
                    type="email"
                    onChange={(e) => {
                      setEmail(e.target.value);
                    }}
                    value={email ?? ""}
                    placeholder={user?.email}
                  />
                </div>
              </div>
              <div className="card-actions justify-end mt-4">
                <button
                  disabled={isLoading}
                  className="btn btn-success"
                  onClick={async () => {
                    setIsLoading(true);
                    if (user) {
                      await setUser({
                        ...user,
                        firstName: firstName ?? user.firstName,
                        lastName: lastName ?? user.lastName,
                        email: email ?? user.email,
                      });
                      setIsOpen(false);
                    }

                    setIsLoading(false);
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
