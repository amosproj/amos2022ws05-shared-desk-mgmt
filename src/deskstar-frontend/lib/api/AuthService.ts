import { BACKEND_URL } from "./constants";

export type AuthResult = string | AuthResponse;

type RegisterUser = {
  mailAddress: String;
  firstName: String;
  lastName: String;
  password: String;
  companyId: String;
};

export enum AuthResponse {
  ErrorUnknown,
  ErrorCompanyAlreadyExists,
  ErrorEmailaddressAlreadyExists,
  ErrorNotAllowedToCreateUser,
  ErrorInvalidCredentials,
  ErrorUserNotApproved,
  ErrorServer,
  Success,
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
    if (response.status == 401) {
      const message = await response.text();

      if (message == "NotYetApproved") {
        return AuthResponse.ErrorUserNotApproved;
      }

      return AuthResponse.ErrorInvalidCredentials;
    }

    return AuthResponse.ErrorServer;
  }

  return response.text();
}

export async function register(user: RegisterUser): Promise<AuthResponse> {
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
        return AuthResponse.ErrorUnknown;

      case 403:
        // not authorized to create a user
        return AuthResponse.ErrorUnknown;
    }

    return AuthResponse.ErrorUnknown;
  }

  return AuthResponse.Success;
}
