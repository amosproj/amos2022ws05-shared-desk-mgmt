import { NextApiRequest, NextApiResponse } from "next";
import { unstable_getServerSession } from "next-auth/next";
import { authOptions } from "./auth/[...nextauth]";
import {
  getBuildings,
  getDesks,
  getFloors,
  getRooms,
} from "../../lib/api/ResourceService";
import dayjs from "dayjs";
import isBetween from "dayjs/plugin/isBetween";

dayjs.extend(isBetween);

export default async function desks(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== "GET") {
    res.status(405).json({ message: "Method not allowed" });
    return;
  }

  const session = await unstable_getServerSession(req, res, authOptions);

  if (!session) {
    res.status(401).json({ message: "Unauthorized" });
    return;
  }

  const { starttime, endtime, building, floor, room } = req.query;

  const start = dayjs(starttime as string);
  const end = dayjs(endtime as string);

  // First get all buildings
  const buildings = await getBuildings(session);

  //   If buildings are specified, filter them out
  const filteredBuildings = buildings.filter((b) => {
    if (b.isMarkedForDeletion) {
      return false;
    }

    if (building) {
      return b.buildingId === building;
    }

    return true;
  });

  //  Get all floors for each building
  const floors = (
    await Promise.all(
      filteredBuildings.map(async (b) =>
        (
          await getFloors(session, b.buildingId)
        ).map((f) => ({ ...f, buildingId: b.buildingId }))
      )
    )
  ).flat();

  //  If floors are specified, filter them out
  const filteredFloors = floors.filter((f) => {
    if (f.isMarkedForDeletion) {
      return false;
    }

    if (floor) {
      return f.floorId === floor;
    }

    return true;
  });

  //  Get all rooms for each floor
  const rooms = (
    await Promise.all(
      filteredFloors.map(async (f) =>
        (
          await getRooms(session, f.floorId)
        ).map((r) => ({ ...r, buildingId: f.buildingId, floorId: f.floorId }))
      )
    )
  ).flat();

  const filteredRooms = rooms.filter((r) => {
    if (r.isMarkedForDeletion) {
      return false;
    }

    if (room) {
      return r.roomId === room;
    }

    return true;
  });

  //  Get all desks for each room
  const desks = (
    await Promise.all(
      filteredRooms.map(async (r) =>
        //  Get all desks for each room
        (
          await getDesks(session, r.roomId)
        ).map((d) => ({
          ...d,
          buildingId: r.buildingId,
          floorId: r.floorId,
          roomId: r.roomId,
        }))
      )
    )
  ).flat();

  // Filter the bookings in the desks for the specified time range
  desks.forEach((d) => {
    d.bookings = d.bookings.filter((b) => {
      const bStart = dayjs(b.startTime);
      const bEnd = dayjs(b.endTime);

      return (
        bStart.isBetween(start, end, "minute", "[]") ||
        bEnd.isBetween(start, end, "minute", "[]") ||
        (bStart.isBefore(start) && bEnd.isAfter(end))
      );
    });
  });

  res.status(200).json(desks);
}
