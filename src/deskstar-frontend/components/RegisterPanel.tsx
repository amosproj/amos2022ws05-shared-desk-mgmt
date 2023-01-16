import { useRouter } from "next/router";
import React, { useState } from "react";
import { AuthResponse, register } from "../lib/api/AuthService";
import Input from "./forms/Input";
import { toast } from "react-toastify";

export default function RegisterPanel() {
  const [company, setCompany] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");

  // const [msg, setMsg] = useState("");

  const [clicked, setClicked] = useState(false);

  const router = useRouter();

  async function submitForm(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setClicked(true);

    if (password !== repeatPassword) {
      toast.error("Passwords must be equal!");

      setClicked(false);
      return;
    }

    const response = await register({
      companyId: company,
      firstName,
      lastName,
      mailAddress: email,
      password,
    });

    setClicked(false);

    if (response !== AuthResponse.Success) {
      switch (response) {
        case AuthResponse.ErrorCompanyNotFound:
          toast.error("Company not Found");
        case AuthResponse.ErrorEmailaddressAlreadyExists:
          toast.error("Email adress already registered");
        default:
          toast.error("Unknown error");
      }
      return;
    }

    document.location = "/login?msg=Registration+successful";
  }

  return (
    <div className="flex flex-col">
      <h1 className="text-3xl font-bold">Register</h1>
      <form className="flex flex-col" onSubmit={submitForm}>
        <Input
          name="company"
          value={company}
          onChange={(e) => setCompany(e.target.value)}
          placeholder="Company"
        />
        <div className="columns-2">
          <Input
            name="firstname"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            placeholder="Firstname"
          />
          <Input
            name="lastname"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            placeholder="Lastname"
          />
        </div>
        <Input
          name="email"
          value={email}
          type="email"
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
        />
        <div className="columns-2">
          <Input
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="Password"
          />
          <Input
            name="repeat_password"
            value={repeatPassword}
            onChange={(e) => setRepeatPassword(e.target.value)}
            type="password"
            placeholder="Repeat password"
          />
        </div>

        {/* <p className="text-green-500 text-center py-4">{msg}</p> */}

        {!clicked ? (
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            type="submit"
          >
            Register
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
