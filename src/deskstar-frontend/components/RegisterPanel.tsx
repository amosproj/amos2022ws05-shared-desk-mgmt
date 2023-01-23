import { useRouter } from "next/router";
import React, { useState } from "react";
import { AuthResponse, register } from "../lib/api/AuthService";
import Input from "./forms/Input";
import { toast } from "react-toastify";
import { Combobox } from "@headlessui/react";

function CompanyCombobox({
  companies,
  selectedId,
  setSelectedId,
}: {
  companies: { id: string; name: string }[];
  selectedId: string;
  setSelectedId: (id: string) => void;
}) {
  const [query, setQuery] = useState("");

  const filteredCompanies = companies.filter((company) =>
    company.name.toLowerCase().includes(query.toLowerCase())
  );

  return (
    <div className="dropdown my-2">
      <Combobox value={selectedId} onChange={setSelectedId}>
        <Combobox.Input
          className="input w-full border-2 border-gray-300 p-2 rounded-lg"
          placeholder="Company"
          onChange={(event) => setQuery(event.target.value)}
        />
        <Combobox.Options className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52">
          {filteredCompanies.map((company) => (
            <Combobox.Option key={company.id} value={company.id}>
              {({ active }) => (
                <a className={active ? "active" : ""}>{company.name}</a>
              )}
            </Combobox.Option>
          ))}
        </Combobox.Options>
      </Combobox>
    </div>
  );
}

export default function RegisterPanel() {
  const [company, setCompany] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");

  // const [msg, setMsg] = useState("");

  const [clicked, setClicked] = useState(false);

  const companies = [
    { id: "1", name: "Company 1" },
    { id: "2", name: "Company 2" },
  ];

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
          break;
        case AuthResponse.ErrorEmailaddressAlreadyExists:
          toast.error("Email adress already registered");
          break;
        default:
          toast.error("Unknown error");
          break;
      }
      return;
    }

    document.location = "/login?msg=Registration+successful";
  }

  return (
    <div className="flex flex-col">
      <h1 className="text-3xl font-bold">Register</h1>
      <form className="flex flex-col" onSubmit={submitForm}>
        {/* <Input
          name="Company"
          value={company}
          onChange={(e) => setCompany(e.target.value)}
          placeholder="Company"
        /> */}
        {company}
        <CompanyCombobox
          selectedId={company}
          setSelectedId={setCompany}
          companies={companies}
        />
        <div className="columns-2">
          <Input
            name="Firstname"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            placeholder="Firstname"
          />
          <Input
            name="Lastname"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            placeholder="Lastname"
          />
        </div>
        <Input
          name="Email"
          value={email}
          type="email"
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
        />
        <div className="columns-2">
          <Input
            name="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="Password"
          />
          <Input
            name="Repeat Password"
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
