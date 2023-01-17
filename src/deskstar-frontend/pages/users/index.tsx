import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../../components/UsersTable";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
import { getUsers, deleteUser, editUser } from "../../lib/api/UserService";
import { authOptions } from "../api/auth/[...nextauth]";
import { unstable_getServerSession } from "next-auth";
import { toast } from "react-toastify";
import ConfirmModal from "../../components/ConfirmModal";
import EditUserModal from "../../components/EditUserModal";

export default function UsersOverview({ users }: { users: IUser[] }) {
  let { data: session } = useSession();

  const [userList, setUserList] = useState(users);

  const [calledRouter, setCalledRouter] = useState(false);
  const router = useRouter();

  const [user, setUser] = useState<IUser>();

  const [isMakeAdminModalOpen, setMakeAdminModalOpen] = useState(false);
  const [isEditUserModalOpen, setEditUserModalOpen] = useState(false);
  const [isDeleteUserModalOpen, setDeleteUserModalOpen] = useState(false);

  // page is only accessable as admin
  useEffect(() => {
    if (!calledRouter && session && !session?.user.isAdmin) {
      // redirect to homepage
      router.push({
        pathname: "/",
      });
      // prevent multiple router pushs
      setCalledRouter(true);
    }
  }, [router, session, calledRouter]);

  if (session == null) {
    return;
  }

  async function onPermissionUpdate(user: IUser) {
    setUser(user);
    setMakeAdminModalOpen(true);
  }

  async function onEdit(user: IUser) {
    setUser(user);
    setEditUserModalOpen(true);
  }

  async function onDelete(user: IUser) {
    setUser(user);
    setDeleteUserModalOpen(true);
  }

  async function doDelete() {
    if (user) {
      if (session == null) return;
      let result = await deleteUser(session, user.userId);

      if (result.ok) {
        toast.success(`User ${user.firstName} ${user.lastName} deleted!`);

        // Remove the user from userList
        setUserList(userList.filter((u) => user.userId !== u.userId));
      } else {
        toast.error(
          `User ${user.firstName} ${user.lastName} could not be deleted!`
        );
      }
    }
  }

  async function doChangePermissions() {
    if (user) {
      if (session == null) return;

      // Toggle isAdmin
      const newUser = {
        ...user,
        isAdmin: !user.isAdmin,
      };

      setUser(newUser);

      let result = await editUser(session, newUser);

      if (result.ok) {
        toast.success(`User ${user.firstName} ${user.lastName} updated!`);

        // Change the user in userList
        setUserList(
          userList.map((u) => (newUser.userId === u.userId ? newUser : u))
        );
      } else {
        toast.error(
          `User ${user.firstName} ${user.lastName} could not be updated!`
        );
      }
    }
  }

  async function doEdit(newUser: IUser) {
    if (session == null) return;

    let result = await editUser(session, newUser);

    if (result.ok) {
      setUser(newUser);

      toast.success(`User ${newUser.firstName} ${newUser.lastName} updated!`);

      // Change the user in userList
      setUserList(
        userList.map((user) =>
          user.userId === newUser.userId ? newUser : user
        )
      );
      return true;
    } else {
      toast.error(
        `User ${newUser.firstName} ${newUser.lastName} could not be updated!`
      );
    }
    return false;
  }

  return (
    <>
      <Head>
        <title>Users Overview</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Users Overview</h1>
      <UsersTable
        users={userList}
        onPermissionUpdate={onPermissionUpdate}
        onEdit={onEdit}
        onDelete={onDelete}
      />
      <ConfirmModal
        title={"Delete User " + user?.firstName + " " + user?.lastName + "?"}
        description="This can't be undone!"
        text=""
        warn
        buttonText="DELETE"
        action={doDelete}
        isOpen={isDeleteUserModalOpen}
        setIsOpen={setDeleteUserModalOpen}
      ></ConfirmModal>
      <ConfirmModal
        title={
          "Change admin rights for User " +
          user?.firstName +
          " " +
          user?.lastName +
          "?"
        }
        description="Please confirm that you want to update the privileges this user an admin."
        text=""
        warn
        buttonText="UPDATE PRIVILEGES"
        action={doChangePermissions}
        isOpen={isMakeAdminModalOpen}
        setIsOpen={setMakeAdminModalOpen}
      ></ConfirmModal>
      <EditUserModal
        user={user}
        setUser={doEdit}
        isOpen={isEditUserModalOpen}
        setIsOpen={setEditUserModalOpen}
      />
    </>
  );
}

//TODO: delete this when using backend data instead of mockup
export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (session) {
    const users = await getUsers(session);

    return {
      props: {
        users: users.filter((user: IUser) => user.isApproved),
      },
    };
  }

  return {
    props: {
      users: [],
    },
  };
};
