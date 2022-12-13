import React, { useState } from "react";
import { IUser } from "../types/users";
import {
  FaTrashAlt,
  FaEdit,
  FaCheckCircle,
  FaTimesCircle,
} from "react-icons/fa";

export function UsersTable({
  users,
  onEdit,
  onDelete,
  onPermissionUpdate,
  onApprovalUpdate,
  onUsersSelection,
}: {
  users: IUser[];
  selectedUsers?: { [userId: string]: boolean };
  onEdit?: (user: IUser) => Promise<void>;
  onDelete?: (user: IUser) => Promise<void>;
  onPermissionUpdate?: (user: IUser) => Promise<void>;
  onApprovalUpdate?: (user: IUser, decision: boolean) => Promise<void>;
  onUsersSelection?: React.Dispatch<React.SetStateAction<IUser[]>>;
}) {
  const [allUsersButtonState, setAllUsersButtonState] = useState(false);

  let toggleUserSelection: (user: IUser) => void;
  let toggleAllUsersSelection: () => void;
  if (onUsersSelection) {
    toggleUserSelection = (selectedUser: IUser) => {
      // update user state
      const updatedUser: IUser = {
        ...selectedUser,
        selected: !selectedUser.selected,
      };

      const updatedUsers = users.map((u: IUser) =>
        u.userId !== selectedUser.userId ? u : updatedUser
      );

      onUsersSelection(updatedUsers);

      // update all users selection button, if needed
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
  }

  return (
    <div className="overflow-x-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {onUsersSelection && (
              <th className="bg-deskstar-green-light">
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
            <th className="bg-deskstar-green-light text-center">First Name</th>
            <th className="bg-deskstar-green-light text-center">LastName</th>
            <th className="bg-deskstar-green-light text-center">E-Mail</th>
            {onPermissionUpdate && (
              <th className="bg-deskstar-green-light text-center">Admin</th>
            )}
            {onApprovalUpdate && (
              <th className="bg-deskstar-green-light text-center">
                Approve/Reject
              </th>
            )}
            {onEdit && <th className="bg-deskstar-green-light"></th>}
            {onDelete && <th className="bg-deskstar-green-light"></th>}
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
              onUserSelection={toggleUserSelection}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
}

const UsersTableEntry = ({
  user,
  onEdit,
  onDelete,
  onPermissionUpdate,
  onApprovalUpdate,
  onUserSelection,
}: {
  user: IUser;
  onEdit?: (user: IUser) => Promise<void>;
  onDelete?: (user: IUser) => Promise<void>;
  onPermissionUpdate?: (user: IUser) => Promise<void>;
  onApprovalUpdate?: (user: IUser, decision: boolean) => Promise<void>;
  onUserSelection?: (user: IUser) => void;
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
            onChange={() => onPermissionUpdate(user)}
            className="checkbox"
          />
        </td>
      )}
      {onApprovalUpdate && (
        <td className="text-center">
          <button
            className="btn btn-ghost"
            onClick={() => onApprovalUpdate(user, true)}
          >
            <FaCheckCircle color="green" />
          </button>
          <button
            className="btn btn-ghost"
            onClick={() => onApprovalUpdate(user, false)}
          >
            <FaTimesCircle color="red" />
          </button>
        </td>
      )}
      {onEdit && (
        <td className="p-0">
          <button className="btn btn-ghost" onClick={() => onEdit(user)}>
            <FaEdit />
          </button>
        </td>
      )}
      {onDelete && (
        <td className="p-0">
          <button className="btn btn-ghost" onClick={() => onDelete(user)}>
            <FaTrashAlt color="red" />
          </button>
        </td>
      )}
    </tr>
  );
};

export default UsersTableEntry;
