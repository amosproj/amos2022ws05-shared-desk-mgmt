import { signIn, useSession } from "next-auth/react";
import Head from "next/head";
import { useState } from "react";
import LoginPanel from "../components/LoginPanel";

export default function Login() {
  return (
    <div className="flex items-center justify-center h-[80vh]">
      <Head>
        <title>Login</title>
      </Head>

      <LoginPanel />
    </div>
  );
}
