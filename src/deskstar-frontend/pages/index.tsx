import { signIn, signOut, useSession } from "next-auth/react";

export default function Home() {
  const { data: session, status } = useSession();
  return (
    <>
      <h1 className="text-3xl font-bold underline">Hello world!</h1>
      {session && (
        <>
          <p className="text-2xl">Welcome {session.user.name}!</p>
          <button onClick={() => signOut()}>Sign out</button>
        </>
      )}
      {!session && (
        <>
          <p className="text-2xl">Not signed in</p>
          <button
            onClick={() => signIn()}
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Sign in
          </button>
        </>
      )}
    </>
  );
}
