import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../components/UsersTable";
import { IUser } from "../types/users";
import { UserManagementWrapper } from "../components/UserManagementWrapper";
//TODO: delete this when using backend data instead of mockup
import { users } from "../users";

export default function UsersOverview({ users }: { users: IUser[] }) {
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
