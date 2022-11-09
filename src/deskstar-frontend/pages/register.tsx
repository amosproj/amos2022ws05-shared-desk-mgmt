import Head from "next/head";
import Link from "next/link";
import RegisterPanel from "../components/RegisterPanel";

export default function Register() {
  return (
    <div className="flex flex-col items-center justify-center h-[80vh] gap-2">
      <Head>
        <title>Register</title>
      </Head>

      <RegisterPanel />

      <Link
        className="underline text-blue-500 hover:text-blue-600"
        href="/login"
      >
        Login
      </Link>
    </div>
  );
}
