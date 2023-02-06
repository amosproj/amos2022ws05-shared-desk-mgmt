import React, { useState } from "react";
import { IUser } from "../types/users";
import {
  FaTrashAlt,
  FaPencilAlt,
  FaCheck,
  FaTimes,
  FaTrashRestore,
} from "react-icons/fa";

export function UsersTable({
  users,
  adminId,
  onEdit,
  onDelete,
  onPermissionUpdate,
  onApprovalUpdate,
  onRestoreUpdate,
  onUsersSelection,
}: {
  users: IUser[];
  adminId: String;
  selectedUsers?: { [userId: string]: boolean };
  onEdit?: (user: IUser) => Promise<void>;
  onDelete?: (user: IUser) => Promise<void>;
  onPermissionUpdate?: (user: IUser) => Promise<void>;
  onApprovalUpdate?: (user: IUser[], decision: boolean) => Promise<void>;
  onRestoreUpdate?: (user: IUser[]) => Promise<void>;
  onUsersSelection?: React.Dispatch<React.SetStateAction<IUser[]>>;
}) {
  const [allUsersButtonState, setAllUsersButtonState] = useState(false);

  let toggleUserSelection: (user: IUser) => void;
  let toggleAllUsersSelection: () => void;
  let selectedUsersCount = 0;
  if (onUsersSelection) {
    toggleUserSelection = (selectedUser: IUser) => {
      const updatedUser: IUser = {
        ...selectedUser,
        selected: !selectedUser.selected,
      };

      const updatedUsers = users.map((u: IUser) =>
        u.userId !== selectedUser.userId ? u : updatedUser
      );
      onUsersSelection(updatedUsers);
      setAllUsersButtonState(
        updatedUsers.reduce((acc: boolean, currUser: IUser): boolean => {
          if (!currUser.selected) return false;
          return acc && currUser.selected;
        }, true)
      );
    };

    toggleAllUsersSelection = () => {
      onUsersSelection(
        users.map((user: IUser) => {
          return { ...user, selected: !allUsersButtonState };
        })
      );
      setAllUsersButtonState(!allUsersButtonState);
    };

    selectedUsersCount = users.reduce(
      (acc: number, currUser: IUser): number => {
        return currUser.selected ? acc + 1 : acc;
      },
      0
    );
  }

  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {onUsersSelection && (
              <th className="bg-secondary">
                <label>
                  <input
                    type="checkbox"
                    className="checkbox dark:border-black"
                    checked={allUsersButtonState}
                    onChange={() => toggleAllUsersSelection()}
                  />
                </label>
              </th>
            )}
            <th className="bg-secondary text-center">First Name</th>
            <th className="bg-secondary text-center">Last Name</th>
            <th className="bg-secondary text-center">E-Mail</th>
            {onPermissionUpdate && (
              <th className="bg-secondary text-center">Admin</th>
            )}
            {onApprovalUpdate && (
              <th className="bg-secondary text-center">Approve/Reject</th>
            )}
            {onRestoreUpdate && (
              <th className="bg-secondary text-center">Restore</th>
            )}
            {onEdit && <th className="bg-secondary"></th>}
            {onDelete && <th className="bg-secondary"></th>}
          </tr>
        </thead>
        <tbody>
          {users.map((user: IUser) => (
            <UsersTableEntry
              key={user.userId}
              user={user}
              onEdit={onEdit}
              onDelete={onDelete}
              onPermissionUpdate={onPermissionUpdate}
              onApprovalUpdate={onApprovalUpdate}
              onRestoreUpdate={onRestoreUpdate}
              onUserSelection={toggleUserSelection}
              isSelf={user.userId === adminId}
            />
          ))}
        </tbody>
      </table>
      {onApprovalUpdate && onUsersSelection && selectedUsersCount > 0 && (
        <div className="mt-10 flex md:justify-center flex-col lg:flex-row">
          <button
            className="btn btn-primary mb-5 lg:mr-5"
            onClick={() =>
              onApprovalUpdate(
                users.filter((u) => u.selected),
                true
              )
            }
          >
            Approve selection
          </button>
          <button
            className="btn btn-error"
            onClick={() =>
              onApprovalUpdate(
                users.filter((u) => u.selected),
                false
              )
            }
          >
            Reject selection
          </button>
        </div>
      )}
      {onRestoreUpdate && onUsersSelection && selectedUsersCount > 0 && (
        <div className="mt-10 flex md:justify-center flex-col lg:flex-row">
          <button
            className="btn bg-green-900 mb-5 lg:mr-5"
            onClick={() => onRestoreUpdate(users.filter((u) => u.selected))}
          >
            Restore selection
          </button>
        </div>
      )}
    </div>
  );
}

const UsersTableEntry = ({
  user,
  onEdit,
  onDelete,
  onPermissionUpdate,
  onApprovalUpdate,
  onRestoreUpdate,
  onUserSelection,
  isSelf,
}: {
  user: IUser;
  onEdit?: (user: IUser) => Promise<void>;
  onDelete?: (user: IUser) => Promise<void>;
  onPermissionUpdate?: (user: IUser) => Promise<void>;
  onApprovalUpdate?: (user: IUser[], decision: boolean) => Promise<void>;
  onRestoreUpdate?: (user: IUser[]) => Promise<void>;
  onUserSelection?: (user: IUser) => void;
  isSelf: boolean;
}) => {
  return (
    <tr className="hover">
      {onUserSelection && (
        <th>
          <label>
            <input
              type="checkbox"
              className="checkbox"
              checked={user.selected}
              onChange={() => onUserSelection(user)}
            />
          </label>
        </th>
      )}
      <td className="text-center">{user.firstName}</td>
      <td className="text-center">{user.lastName}</td>
      <td className="text-center">{user.email}</td>
      {onPermissionUpdate && (
        <td className="text-center">
          <input
            type="checkbox"
            checked={user.isAdmin}
            disabled={isSelf}
            onChange={() => onPermissionUpdate(user)}
            className="checkbox"
          />
        </td>
      )}
      {onApprovalUpdate && (
        <td className="text-center flex gap-2 justify-center">
          <button
            className="btn btn-ghost"
            onClick={() => onApprovalUpdate([user], true)}
          >
            <FaCheck color="green" />
          </button>
          <button
            className="btn btn-ghost"
            onClick={() => onApprovalUpdate([user], false)}
          >
            <FaTimes color="red" />
          </button>
        </td>
      )}
      {onRestoreUpdate && (
        <td className="text-center">
          <button
            className="btn btn-ghost"
            onClick={() => onRestoreUpdate([user])}
          >
            <FaTrashRestore color="green" />
          </button>
        </td>
      )}
      {onEdit && (
        <td className="p-0">
          <button className="btn btn-ghost" onClick={() => onEdit(user)}>
            <FaPencilAlt />
          </button>
        </td>
      )}
      {onDelete && (
        <td className="p-0">
          {!isSelf && (
            <button className="btn btn-ghost" onClick={() => onDelete(user)}>
              <FaTrashAlt color="red" />
            </button>
          )}
          {isSelf && (
            <button className="btn btn-ghost no-animation">
              <FaTrashAlt color="grey" />
            </button>
          )}
        </td>
      )}
    </tr>
  );
};

export default UsersTableEntry;
