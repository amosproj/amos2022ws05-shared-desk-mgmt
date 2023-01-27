import dayjs, { Dayjs } from "dayjs";
import { Session } from "next-auth";
import { URLSearchParams } from "next/dist/compiled/@edge-runtime/primitives/url";
import { IDesk } from "../../types/desk";

/**
 * Returns list of aggregated desks
 * @returns The list of aggregated desks
 * @throws Error containing status code and/or error message
 */
export async function getAggregatedDesks(
  session: Session,
  starttime: Dayjs,
  endtime: Dayjs,
  building?: string,
  floor?: string,
  room?: string
): Promise<IDesk[]> {
  const params = new URLSearchParams({
    starttime: starttime.unix().toString(),
    endtime: endtime.unix().toString(),
  });

  if (building) params.append("building", building);
  if (floor) params.append("floor", floor);
  if (room) params.append("room", room);

  const response = await fetch(`/api/desks?${params.toString()}`);

  if (!response.ok) throw Error(`${response.status} ${response.statusText}`);

  return response.json();
}
