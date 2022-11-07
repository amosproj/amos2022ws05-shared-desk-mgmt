import { signIn, signOut, useSession } from "next-auth/react";
import { useState } from "react";
import Input from "./forms/Input";

export default function LoginPanel() {
  const { data: session } = useSession();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [company, setCompany] = useState("");

  return (
    <div className="flex flex-col">
      {session && (
        <div className="flex flex-col gap-2">
          <p className="">You are already logged in as {session.user?.name}!</p>
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            onClick={() => signOut()}
          >
            Sign out
          </button>
        </div>
      )}

      {!session && (
        <>
          <h1 className="text-3xl font-bold">Login</h1>
          <form
            className="flex flex-col"
            onSubmit={(e) => {
              e.preventDefault();
              signIn("credentials", { company, email, password });
            }}
          >
            <Input
              name="company"
              value={company}
              onChange={(e) => setCompany(e.target.value)}
              placeholder="Company"
            />
            <Input
              name="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email"
            />
            <Input
              name="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              type="password"
            />

            <button
              className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
              type="submit"
            >
              Login
            </button>
          </form>
        </>
      )}
    </div>
  );
}
