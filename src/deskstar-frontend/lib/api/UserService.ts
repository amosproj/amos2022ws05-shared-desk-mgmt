import { Session } from "next-auth";
import { IUser } from "../../types/users"
import { BACKEND_URL } from "./constants";

export async function getUsers(session: Session): Promise<IUser[]> {
  //   const response = await fetch(BACKEND_URL + "/resources/locations");
  const response = await fetch(BACKEND_URL + "/users", {
    headers: {
      Authorization: `Bearer ${session.accessToken}`,
    },
  });

  if(response.status !== 200){
    console.log("Error fetching ")
    return [];
  }

  const data = await response.json();
  return data;
}