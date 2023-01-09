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
import  ConfirmModal from "../../components/ConfirmModal";
import  EditUserModal  from "../../components/EditUserModal";


export default function UsersOverview({ users }: { users: IUser[] }) {
  let { data: session } = useSession();
  const [calledRouter, setCalledRouter] = useState(false);
  const router = useRouter();
  
  const [user, setUser] = useState<IUser>();

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
    setUser(user);
  };

  async function onDelete(user: IUser) {
    //Show aller
  }
  const doDelete = async (user: IUser) => {
    console.log(`Deleting user ${user.userId}...`);
    if (session == null) return;
    deleteUser(session, user.userId);
  };
  const saveEdit= async (user:IUser)=>{
    console.log(`Edit user ${user.userId}...`);
    //send edit
  }

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
      <ConfirmModal title="Delete User"  description="This can&apos;t be undone!" text="" warn buttonText="DELETE" action={doDelete}></ConfirmModal>
      {/*<EditUserModal user={user} action={saveEdit}/>*/}
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
