import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../../components/UsersTable";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
import { getUsers } from "../../lib/api/UserService";
import { authOptions } from "../api/auth/[...nextauth]";
import { unstable_getServerSession } from "next-auth";

export default function UserRequests({ users }: { users: IUser[] }) {
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

  const onApprovalUpdate = async (
    user: IUser,
    decision: boolean
  ): Promise<void> => {
    //TODO: Implement
    if (decision) console.log(`Approving user ${user.userId}...`);
    else console.log(`Rejecting user ${user.userId}...`);
  };

  if (!session?.user?.isAdmin) {
    //TODO: Add loading animation
    return <div>Loading...</div>;
  }

  return (
    <>
      <Head>
        <title>User Requests</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">User Requests</h1>
      <UsersTable users={users} onApprovalUpdate={onApprovalUpdate} />
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
        users: users.filter((user: IUser) => !user.isApproved),
      },
    };
  }

  return {
    props: {
      users: [],
    },
  };
};
