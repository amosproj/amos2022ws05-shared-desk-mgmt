import { BACKEND_URL } from "./constants";

/**
 * Returns list of registered companies
 * @returns The list of companies
 * @throws Error containing status code and/or error message
 */
export async function getCompanies() {
  const response = await fetch(`${BACKEND_URL}/companies`);

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response.json();
}
