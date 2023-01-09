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

export default function UsersOverview({ users }: { users: IUser[] }) {
  let { data: session } = useSession();
  const [calledRouter, setCalledRouter] = useState(false);
  const router = useRouter();

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

  const onPermissionUpdate = async (user: IUser): Promise<void> => {
    //TODO: Implement
    if (user.isAdmin) console.log(`Demoting user ${user.userId}...`);
    else console.log(`Promoting user ${user.userId}...`);
  };

  const onEdit = async (user: IUser): Promise<void> => {
    //TODO: Implement
    console.log(`Editing user ${user.userId}...`);
  };

  async function onDelete(user: IUser) {
    //Show aller
  }
  const doDelete = async (user: IUser) => {
    console.log(`Deleting user ${user.userId}...`);
    if (session == null) return;
    deleteUser(session, user.userId);
  };

  return (
    <>
      <Head>
        <title>Users Overview</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Users Overview</h1>
      <UsersTable
        users={users}
        onPermissionUpdate={onPermissionUpdate}
        onEdit={onEdit}
        onDelete={onDelete}
      />
      <div className="alert alert-warning shadow-lg">
        <div>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="stroke-current flex-shrink-0 h-6 w-6"
            fill="none"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
            />
          </svg>
          <span>Warning: This can&apos;t be undone!</span>
          <div className="flex-none">
            <button className="btn btn-sm btn-ghost" onClick={() => doDelete}>
              DELETE
            </button>
          </div>
        </div>
      </div>
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
