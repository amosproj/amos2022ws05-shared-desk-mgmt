import { signIn, useSession } from "next-auth/react";
import { useState } from "react";
import Input from "./forms/Input";

export default function LoginPanel() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [company, setCompany] = useState("");
  const [clicked, setClicked] = useState(false);

  const onSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    setClicked(true);
    const _ = await signIn("credentials", {
      company,
      email,
      password,
      redirect: false,
    });
    //TODO: display error on status != 200
    setClicked(false);
  };

  return (
    <div className="flex flex-col">
      <h1 className="text-3xl font-bold">Login</h1>
      <form className="flex flex-col" onSubmit={onSubmit}>
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
        {!clicked ? (
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            type="submit"
          >
            Login
          </button>
        ) : (
          <button
            className="btn bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded loading"
            type="submit"
          >
            Loading
          </button>
        )}
      </form>
    </div>
  );
}
