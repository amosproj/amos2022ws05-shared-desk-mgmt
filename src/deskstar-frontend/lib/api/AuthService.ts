import { BACKEND_URL } from "./constants";

export type AuthResult = string | AuthError;

type RegisterUser = {
  mailAddress: String;
  firstName: String;
  lastName: String;
  password: String;
  companyId: String;
};

export enum AuthError {
  UnknownError,
  CompanyAlreadyExists,
  EmailaddressAlreadyExists,
  NotAllowedToCreateUser,
  InvalidCredentials,
}

export async function authorize(
  mail: String,
  password: String
): Promise<AuthResult> {
  const response = await fetch(BACKEND_URL + "/auth/createToken", {
    method: "POST",
    body: JSON.stringify({
      mailAddress: mail,
      password,
    }),
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (response.status != 200) {
    return AuthError.InvalidCredentials;
  }

  return response.text();
}

export async function register(user: RegisterUser): Promise<AuthResult> {
  const response = await fetch(BACKEND_URL + "/auth/register", {
    method: "POST",
    body: JSON.stringify(user),
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (response.status != 200) {
    switch (response.status) {
      case 400:
        // Error: company does not exist
        // Error: mail address already exists
        return AuthError.UnknownError;

      case 403:
        // not authorized to create a user
        return AuthError.UnknownError;
    }

    return AuthError.UnknownError;
  }

  return await response.text();
}
