import Head from "next/head";
import { GetServerSideProps } from "next";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
//TODO: delete this when using backend data instead of mockup
import { users } from "../../users";
import ResourceManagementTable from "../../components/ResourceManagementTable";
import DropDownFilter, {
  stringToSelectable,
} from "../../components/DropDownFilter";
import { desks } from "../../desks";
import { IDesk } from "../../types/desk";
import { rooms } from "../../rooms";
import { IRoom } from "../../types/room";

export default function ResourcesOverview({ results }: { results: IRoom[] }) {
  const { data: session } = useSession();

  const [chosenResources, setChosenResources] = useState<IDesk[]>(desks);
  const router = useRouter();

  let buildings: string[] = [];
  let locations: string[] = [];
  let floors: string[] = [];
  let rooms: string[] = [];

  for (const resource of results) {
    buildings.push(resource.building);
    locations.push(resource.location);
    floors.push(resource.floor);
    rooms.push(resource.roomName);
  }

  // page is only accessable as admin
  useEffect(() => {
    if (session && !session?.user.isAdmin) {
      // redirect to homepage
      router.push({
        pathname: "/",
      });
    }
  }, [router, session]);

  const onEdit = async (desk: IDesk): Promise<void> => {
    //TODO: Implement
    console.log(`Editing desk ${desk.deskId}...`);
  };

  const onDelete = async (desk: IDesk): Promise<void> => {
    //TODO: Implement
    console.log(`Deleting desk ${desk.deskId}...`);
  };

  return (
    <>
      <Head>
        <title>Resources Overview</title>
      </Head>

      <DropDownFilter
        title="Locations"
        options={stringToSelectable(locations)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected locations: ", selectedOptions);
        }}
      />
      <DropDownFilter
        title="Buildings"
        options={stringToSelectable(buildings)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected buildings: ", selectedOptions);
        }}
      />
      <DropDownFilter
        title="Floors"
        options={stringToSelectable(floors)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected floors: ", selectedOptions);
        }}
      />
      <DropDownFilter
        title="Rooms"
        options={stringToSelectable(rooms)}
        setSelectedOptions={(selectedOptions) => {
          console.log("Selected rooms: ", selectedOptions);
        }}
      />

      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-left my-10">
          Resources Overview
        </h1>
        <button
          type="button"
          className="btn btn-secondary bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light"
          onClick={() => {}}
        >
          Add Resource
        </button>
      </div>

      <ResourceManagementTable
        onEdit={onEdit}
        onDelete={onDelete}
        desks={chosenResources}
      />
    </>
  );
}

//TODO: delete this when using backend data instead of mockup
export const getServerSideProps: GetServerSideProps = async () => {
  return {
    props: {
      results: rooms,
      users: users.filter((user: IUser) => user.isApproved),
    },
  };
};
