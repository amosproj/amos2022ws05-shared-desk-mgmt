import { signIn, useSession } from "next-auth/react";
import { useRouter } from "next/router";
import Head from "next/head";
import Link from "next/link";
import { useState, useEffect } from "react";
import LoginPanel from "../components/LoginPanel";

export default function Login() {
  const { data: session } = useSession();
  const router = useRouter();

  const { msg } = router.query;

  useEffect(() => {
    if (session) {
      // redirect to homepage
      router.push({
        pathname: "/",
      });
    }
  }, [router, session]);

  if (session)
    //TODO: add nice loading component
    return <div>Loading...</div>;

  return (
    <div className="flex flex-col items-center justify-center h-[80vh] gap-2">
      <Head>
        <title>Login</title>
      </Head>

      <p>Message: {msg}</p>

      <LoginPanel />

      <Link
        className="underline text-blue-500 hover:text-blue-600"
        href="/register"
      >
        Register
      </Link>
    </div>
  );
}
