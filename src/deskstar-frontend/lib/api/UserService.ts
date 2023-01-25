import { Session } from "next-auth";
import { IUser } from "../../types/users";
import { BACKEND_URL } from "./constants";

/**
 * Lists all users associated with users company
 * @param session The associated user session
 * @returns All users associated with the given usersessions company
 * @throws Error containing status code and/or error message
 */
export async function getUsers(session: Session): Promise<IUser[]> {
  //   const response = await fetch(BACKEND_URL + "/resources/locations");
  const response = await fetch(BACKEND_URL + "/users", {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

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
      isMarkedForDeletion: userData?.isMarkedForDeletion,
    };
  });
}

/**
 * Approves an user by `userId`
 * @param session The associated user session
 * @param userId The user id
 * @returns The fetch response object
 * @throws Error containing status code and/or error message
 */
export async function approveUser(
  session: Session,
  userId: string
): Promise<Response> {
  const response = await fetch(BACKEND_URL + `/users/${userId}/approve`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response;
}

/**
 * Declines an user by `userId`
 * @param session The associated user session
 * @param userId The user id
 * @returns The fetch response
 * @throws Error containing status code and/or error message
 */
export async function declineUser(
  session: Session,
  userId: string
): Promise<Response> {
  const response = await fetch(BACKEND_URL + `/users/${userId}/decline`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });
  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response;
}

/**
 * Deletes an user by `userId`
 * @param session The associated user session
 * @param userId The user id
 * @returns The fetch response
 * @throws Error containing status code and/or error message
 */
export async function deleteUser(
  session: Session,
  userId: string
): Promise<Response> {
  const response = await fetch(BACKEND_URL + `/users/${userId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response;
}

/**
 * Updates an user
 * @param session The associated user session
 * @param user The updated user data
 * @returns The fetch response
 * @throws Error containing status code and/or error message
 */
export async function editUser(
  session: Session,
  user: IUser
): Promise<Response> {
  const response = await fetch(BACKEND_URL + `/users/edit`, {
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
      isMarkedForDeletion: user.isMarkedForDeletion,
      companyId: user.company,
    }),
  });

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response;
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

/**
 * Get user from accessToken
 * @param accessToken The jwt token received by the backend
 * @returns The user associated with `accessToken`
 * @throws Error containing status code and/or error message
 */
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

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response.json();
}
