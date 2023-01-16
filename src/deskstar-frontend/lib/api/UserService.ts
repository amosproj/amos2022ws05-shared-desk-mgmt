import { Session } from "next-auth";
import { IUser } from "../../types/users";
import { BACKEND_URL } from "./constants";

export async function getUsers(session: Session): Promise<IUser[]> {
  //   const response = await fetch(BACKEND_URL + "/resources/locations");
  const response = await fetch(BACKEND_URL + "/users", {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (response.status !== 200) {
    console.log("Error fetching ");
    return [];
  }

  const data = await response.json();

  return data.map((userData: any): IUser => {
    return {
      userId: userData?.userId,
      firstName: userData?.firstName,
      lastName: userData?.lastName,
      email: userData?.mailAddress,
      company: userData?.companyId,
      isAdmin: userData?.isCompanyAdmin,
      isApproved: userData?.isApproved,
    };
  });
}

export function approveUser(
  session: Session,
  userId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/users/${userId}/approve`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export function declineUser(
  session: Session,
  userId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/users/${userId}/decline`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export async function deleteUser(
  session: Session,
  userId: string
): Promise<Response> {
  return fetch(BACKEND_URL + `/users/delete/${userId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
}

export async function editUser(
  session: Session,
  user: IUser
): Promise<Response> {
  return await fetch(BACKEND_URL + `/users/edit`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      UserId: user.userId,
      LastName: user.lastName,
      FirstName: user.firstName,
      mailAddress: user.email,
      isCompanyAdmin: user.isAdmin,
      companyId: user.company,
    }),
  });
}

type UserResponse = {
  userId: string;
  firstName: string;
  lastName: string;
  mailAddress: string;
  companyId: string;
  isCompanyAdmin: boolean;
  isApproved: boolean;
  company: {
    companyId: string;
    companyName: string;
    logo: boolean;
  };
};

export async function getUserMe(
  accessToken: String
): Promise<UserResponse | undefined> {
  const response = await fetch(BACKEND_URL + `/users/me`, {
    method: "GET",
    next: {
      revalidate: 10 * 60, // Only fetch the data after ten minute
    },
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  return response.json();
}
