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
    if (!user || !session) return toast.error("Error: Couldn't delete user!");

    try {
      await deleteUser(session, user.userId);
      toast.success(`User ${user.firstName} ${user.lastName} deleted!`);
      // Remove the user from userList
      setUserList(userList.filter((u) => user.userId !== u.userId));
    } catch (error) {
      toast.error(`${error}`);
    }
  }

  async function doChangePermissions() {
    if (!user || !session) return;

    // Toggle isAdmin
    const newUser = {
      ...user,
      isAdmin: !user.isAdmin,
    };

    setUser(newUser);

    try {
      await editUser(session, newUser);
      toast.success(`User ${user.firstName} ${user.lastName} updated!`);

      // Change the user in userList
      setUserList(
        userList.map((u) => (newUser.userId === u.userId ? newUser : u))
      );
    } catch (error) {
      toast.error(`${error}`);
    }
  }

  async function doEdit(newUser: IUser): Promise<boolean> {
    if (session == null) return false;

    try {
      await editUser(session, newUser);
      setUser(newUser);

      toast.success(`User ${newUser.firstName} ${newUser.lastName} updated!`);

      // Change the user in userList
      setUserList(
        userList.map((user) =>
          user.userId === newUser.userId ? newUser : user
        )
      );
      return true;
    } catch (error) {
      toast.error(`${error}`);
      return false;
    }
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
        adminId={session.user.id}
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

export const getServerSideProps: GetServerSideProps = async (context) => {
  const session = await unstable_getServerSession(
    context.req,
    context.res,
    authOptions
  );

  if (!session)
    return {
      redirect: {
        destination: "/login",
        permanent: false,
      },
    };

  try {
    const users = await getUsers(session);

    return {
      props: {
        users: users.filter(
          (user: IUser) => user.isApproved && !user.isMarkedForDeletion
        ),
      },
    };
  } catch (error) {
    console.error(error);
    return {
      redirect: {
        destination: "/500",
        permanent: false,
      },
    };
  }
};
