import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../../components/UsersTable";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
import { getUsers, approveUser, declineUser } from "../../lib/api/UserService";
import { authOptions } from "../api/auth/[...nextauth]";
import { unstable_getServerSession } from "next-auth";
import { toast } from "react-toastify";

export default function UserRequests({
  initialUsers,
}: {
  initialUsers: IUser[];
}) {
  const { data: session } = useSession();
  const [calledRouter, setCalledRouter] = useState(false);
  const router = useRouter();
  const [users, setUsers] = useState(
    initialUsers.map((user: IUser): IUser => {
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

  const onApprovalUpdate = async (
    selectedUsers: IUser[],
    decision: boolean
  ): Promise<void> => {
    if (!session) return;
    try {
      for (const user of selectedUsers) {
        const response: Response = decision
          ? await approveUser(session, user.userId)
          : await declineUser(session, user.userId);
      }

      // success
      toast.success(
        `${selectedUsers.length > 1 ? "Users" : "User"} successfully ${
          decision ? "approved" : "rejected"
        }!`
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
      {users.length === 0 && (
        <p className="text-center text-xl">No user requests</p>
      )}
      {users.length > 0 && (
        <UsersTable
          users={users}
          adminId={session.user.id}
          onApprovalUpdate={onApprovalUpdate}
          onUsersSelection={setUsers}
        />
      )}
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
        initialUsers: users.filter((user: IUser) => !user.isApproved),
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
