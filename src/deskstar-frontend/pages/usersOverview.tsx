import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../components/UsersTable";
import { IUser } from "../types/users";
import { UserManagementWrapper } from "../components/UserManagementWrapper";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
//TODO: delete this when using backend data instead of mockup
import { users } from "../users";

export default function UsersOverview({ users }: { users: IUser[] }) {
  const { data: session } = useSession();
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

  const onPermissionUpdate = async (user: IUser): Promise<void> => {
    //TODO: Implement
    if (user.isAdmin) console.log(`Demoting user ${user.userId}...`);
    else console.log(`Promoting user ${user.userId}...`);
  };

  const onEdit = async (user: IUser): Promise<void> => {
    //TODO: Implement
    console.log(`Editing user ${user.userId}...`);
  };

  const onDelete = async (user: IUser): Promise<void> => {
    //TODO: Implement
    console.log(`Deleting user ${user.userId}...`);
  };

  if (!session?.user?.isAdmin) {
    //TODO: Add loading animation
    return <div>Loading...</div>;
  }

  return (
    <UserManagementWrapper>
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
    </UserManagementWrapper>
  );
}

//TODO: delete this when using backend data instead of mockup
export const getServerSideProps: GetServerSideProps = async () => {
  return {
    props: {
      users: users.filter((user: IUser) => user.isApproved),
    },
  };
};
