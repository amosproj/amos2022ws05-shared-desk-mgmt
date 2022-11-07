import { signIn, useSession } from "next-auth/react";
import Head from "next/head";
import Link from "next/link";
import { useState } from "react";
import LoginPanel from "../components/LoginPanel";

export default function Login() {
  return (
    <div className="flex flex-col items-center justify-center h-[80vh] gap-2">
      <Head>
        <title>Login</title>
      </Head>

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
