import Head from "next/head";
import { GetServerSideProps } from "next";
import { IUser } from "../../types/users";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
//TODO: delete this when using backend data instead of mockup
import { users } from "../../users";
import ResourceManagementTable from "../../components/ResourceManagementTable";
import { DropDownFilter } from "../../components/DropDownFilter";
import { desks } from "../../desks";
import { IDesk } from "../../types/desk";

export default function ResourcesOverview({ resources }: { resources: IDesk[] }) {
    const { data: session } = useSession();
    const [calledRouter, setCalledRouter] = useState(false);
    const [chosenResources, setChosenResources] = useState<IDesk[]>(desks);
    const [chosenLocations, _] = useState<Set<string>>(new Set<string>());
    const [chosenBuildings, __] = useState<Set<string>>(new Set<string>());
    const [chosenRooms, ___] = useState<Set<string>>(new Set<string>());
    const [chosenFloors, ____] = useState<Set<string>>(new Set<string>());
    const router = useRouter();

    let buildings: Set<string> = new Set<string>();
    let locations: Set<string> = new Set<string>();
    let floors: Set<string> = new Set<string>();
    let rooms: Set<string> = new Set<string>();

    for (const resource of chosenResources) {
        buildings.add(resource.building);
        locations.add(resource.location);
        floors.add(resource.floor);
        rooms.add(resource.room);
    }

    // page is only accessable as admin
    useEffect(() => {
        if (!calledRouter && session && !session?.user.isAdmin) {
            // redirect to homepage
            router.push({
                pathname: "/",
            });
            // prevent multiple router pushs
            setCalledRouter(true);
        }
    }, [router, session, calledRouter]);

    const onEdit = async (desk: IDesk): Promise<void> => {
        //TODO: Implement
        console.log(`Editing desk ${desk.deskId}...`);
    };

    const onDelete = async (desk: IDesk): Promise<void> => {
        //TODO: Implement
        console.log(`Deleting desk ${desk.deskId}...`);
    };

    const checkSetAgainstFilter = (set: Set<string>, filter: string): boolean => {
        if (set.size === 0) {
            return true;
        }

        return set.has(filter);
    }

    // compares the available resources to the filters and sets the chosen resources accordingly
    const updateResources = () => {
        if (chosenLocations.size === 0 && chosenRooms.size === 0 && chosenBuildings.size === 0 && chosenFloors.size === 0) {
            setChosenResources(desks);
            return;
        }

        let chosen: IDesk[] = [];
        desks.forEach((d) => {
            if (checkSetAgainstFilter(chosenLocations, d.location)
                && checkSetAgainstFilter(chosenBuildings, d.building)
                && checkSetAgainstFilter(chosenFloors, d.floor)
                && checkSetAgainstFilter(chosenRooms, d.room)) {
                chosen.push(d);
            }
        });

        setChosenResources(chosen);
    }

    const updateFilter = (set: Set<string>): ((selection: string[]) => any) => {
        return (selection) => {
            set.clear();
            selection.forEach((s) => set.add(s));
            updateResources();
        };
    }

    return (
        <>
            <Head>
                <title>Resources Overview</title>
            </Head>

            <DropDownFilter title="Locations" options={Array.from(locations)} onSelectionChanged={updateFilter(chosenLocations)} />
            <DropDownFilter title="Buildings" options={Array.from(buildings)} onSelectionChanged={updateFilter(chosenBuildings)} />
            <DropDownFilter title="Floors" options={Array.from(floors)} onSelectionChanged={updateFilter(chosenFloors)} />
            <DropDownFilter title="Rooms" options={Array.from(rooms)} onSelectionChanged={updateFilter(chosenRooms)} />

            <div className="flex items-center justify-between">
                <h1 className="text-3xl font-bold text-left my-10">Resources Overview</h1>
                <button type="button" className="btn btn-secondary bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light" onClick={() => { }}>
                    Add Resource
                </button>
            </div>

            <ResourceManagementTable onEdit={onEdit} onDelete={onDelete} desks={chosenResources} />
        </>
    );
}

//TODO: delete this when using backend data instead of mockup
export const getServerSideProps: GetServerSideProps = async () => {
    return {
        props: {
            users: users.filter((user: IUser) => user.isApproved),
        },
    };
};
