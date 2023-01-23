import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../../components/UsersTable";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
import { getUsers, editUser } from "../../lib/api/UserService";
import { authOptions } from "../api/auth/[...nextauth]";
import { unstable_getServerSession } from "next-auth";
import { toast } from "react-toastify";

export default function DeletedUserOverview({
  deletedUsers,
}: {
  deletedUsers: IUser[];
}) {
  const { data: session } = useSession();
  const [calledRouter, setCalledRouter] = useState(false);
  const router = useRouter();
  const [users, setUsers] = useState(
    deletedUsers.map((user: IUser): IUser => {
      return { selected: false, ...user };
    })
  );

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

  const onRestoreUpdate = async (selectedUsers: IUser[]): Promise<void> => {
    if (!session) return;
    try {
      for (const user of selectedUsers) {
        const response: Response = await restoreUser(user);
      }

      // success
      toast.success(
        `${selectedUsers.length > 1 ? "Users" : "User"} successfully restored!`
      );
      setUsers(
        users.filter(
          (u) => !selectedUsers.map((u2) => u2.userId).includes(u.userId)
        )
      );
    } catch (error) {
      toast.error(`${error}`);
    }
  };

  const restoreUser = async (user: IUser): Promise<void> => {
    if (!session) return;
    user.isMarkedForDeletion = false;
    try {
      const response: Response = await editUser(session, user);
    } catch (error) {
      console.log(error);
    }
  };

  if (!session?.user?.isAdmin) {
    //TODO: Add loading animation
    return <div>Loading...</div>;
  }

  return (
    <>
      <Head>
        <title>Archived Users</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Archived Users</h1>
      <UsersTable
        users={users}
        onRestoreUpdate={onRestoreUpdate}
        onUsersSelection={setUsers}
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
        deletedUsers: users.filter((user: IUser) => user.isMarkedForDeletion),
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
