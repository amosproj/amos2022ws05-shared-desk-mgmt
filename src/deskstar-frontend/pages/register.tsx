import Head from "next/head";
import RegisterPanel from "../components/RegisterPanel";

export default function Register() {
  return (
    <div className="flex items-center justify-center h-[80vh]">
      <Head>
        <title>Register</title>
      </Head>

      <RegisterPanel />
    </div>
  );
}
