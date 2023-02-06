import React, { useState } from "react";
import {
  AuthResponse,
  register,
  initialRegister,
} from "../lib/api/AuthService";
import Input from "./forms/Input";
import { toast } from "react-toastify";
import OwnCombobox from "./forms/Combobox";

type Company = {
  id: string;
  name: string;
};

export default function RegisterPanel({ companies }: { companies: Company[] }) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");
  const [initialRegistration, setInitialRegistration] = useState(false);
  const [newCompanyName, setNewCompanyName] = useState("");

  const [clicked, setClicked] = useState(false);
  const [company, setCompany] = useState<Company | null>(null);

  async function submitForm(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setClicked(true);

    if (password !== repeatPassword) {
      toast.error("Passwords must be equal!");

      setClicked(false);
      return;
    }

    if (!initialRegistration && !company) {
      toast.error("Please select a company");

      setClicked(false);
      return;
    }

    if (initialRegistration && !newCompanyName) {
      toast.error("Please enter your company name");

      setClicked(false);
      return;
    }

    if (initialRegistration) {
      try {
        const response = await initialRegister({
          companyName: newCompanyName,
          firstName,
          lastName,
          mailAddress: email,
          password,
        });
      } catch (error) {
        toast.error(`${error}`);
        setClicked(false);
        return;
      }

      setClicked(false);
      toast.success(
        "Registration successful! We will contact you shortly to activate your account."
      );
      setTimeout(() => {
        document.location = "/login";
      }, 1000);
      return;
    }

    const response = await register({
      companyId: company?.id ?? "",
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

    setClicked(false);
    toast.success("Registration successful!");
    setTimeout(() => {
      document.location = "/login";
    }, 1000);
  }

  return (
    <div className="flex flex-col">
      <h1 className="text-3xl font-bold mb-5">Register</h1>
      <form className="flex flex-col" onSubmit={submitForm}>
        <Input
          type="checkbox"
          name="I want to initially register my company"
          checked={initialRegistration}
          onChange={() => setInitialRegistration(!initialRegistration)}
        />
        {initialRegistration ? (
          <Input
            name="Your Company"
            value={newCompanyName}
            onChange={(e) => setNewCompanyName(e.target.value)}
            placeholder="Company Name"
          />
        ) : (
          <OwnCombobox
            selected={company}
            setSelected={setCompany}
            entities={companies}
            getName={(entity) => entity?.name ?? ""}
          />
        )}
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
            className="btn btn-info text-white font-bold py-2 px-4 rounded"
            type="submit"
          >
            Register
          </button>
        ) : (
          <button
            className="btn btn-info text-white font-bold py-2 px-4 rounded loading"
            type="submit"
          >
            Loading
          </button>
        )}
      </form>
    </div>
  );
}
