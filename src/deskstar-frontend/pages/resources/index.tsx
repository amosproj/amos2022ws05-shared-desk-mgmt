import Head from "next/head";
import { GetServerSideProps } from "next";
import { UsersTable } from "../../components/UsersTable";
import { IUser } from "../../types/users";
import { UserManagementWrapper } from "../../components/UserManagementWrapper";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useState, useEffect } from "react";
//TODO: delete this when using backend data instead of mockup
import { users } from "../../users";
import ResourceManagementTable from "../../components/ResourceManagementTable";
import { desks } from "../../desks";
import { IDesk } from "../../types/desk";

export default function ResourcesOverview({ resources }: { resources: IDesk[] }) {
    resources = desks;

    const { data: session } = useSession();
    const [calledRouter, setCalledRouter] = useState(false);
    const [chosenResources, setChosenResources] = useState<IDesk[]>(desks);
    const [chosenLocations, setChosenLocations] = useState<Set<string>>(new Set<string>());
    const [chosenBuildings, setChosenBuildings] = useState<Set<string>>(new Set<string>());
    const [chosenRooms, setChosenRooms] = useState<Set<string>>(new Set<string>());
    const router = useRouter();

    let buildings: Set<string> = new Set<string>();
    let locations: Set<string> = new Set<string>();
    let rooms: Set<string> = new Set<string>();

    for (const resource of resources) {
        buildings.add(resource.building);
        locations.add(resource.location);
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

    const updateResources = () => {
        let chosen: IDesk[] = [];

        if (chosenLocations.size === 0 && chosenRooms.size === 0 && chosenBuildings.size === 0) {
            setChosenResources(desks);
            return;
        }

        desks.forEach((d) => {
            chosenLocations.forEach((l) => {
                if (d.location === l && !chosen.includes(d))
                    chosen.push(d);
            });

            chosenRooms.forEach((r) => {
                if (d.room === r && !chosen.includes(d))
                    chosen.push(d);
            });

            chosenBuildings.forEach((b) => {
                if (d.building === b && !chosen.includes(d))
                    chosen.push(d);
            });
        })
        setChosenResources(chosen);
        console.log(chosenResources);
    }

    if (!session?.user?.isAdmin) {
        //TODO: Add loading animation
        return <div>Loading...</div>;
    }

    return (
        <>
            <Head>
                <title>Resources Overview</title>
            </Head>

            <div className="dropdown dropdown-hover">
                <label tabIndex={0} className="btn m-1">
                    Locations
                </label>
                <div className="form-control">
                    <ul
                        tabIndex={0}
                        className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
                    >
                        <li key="li_allLocations">
                            <label className="label cursor-pointer">
                                <span className="label-text">All Locations</span>
                                <input
                                    id="allLocations"
                                    type="checkbox"
                                    className="checkbox"
                                    onClick={() => {
                                        locations.forEach((location) => {
                                            var ebox = document.getElementById(location);
                                            if (ebox != null && ebox instanceof HTMLInputElement) {
                                                var box: HTMLInputElement = ebox;
                                                if (!box.checked) {
                                                    chosenLocations.add(location);
                                                    updateResources();
                                                }
                                                box.checked = true;
                                            }
                                        });
                                        updateResources();
                                    }}
                                />
                            </label>
                        </li>
                        <div className="divider"></div>
                        {Array.from(locations).map((location: string) => (
                            <li key={"li_" + location}>
                                <label className="label cursor-pointer">
                                    <span className="label-text">{location}</span>
                                    <input
                                        id={location}
                                        type="checkbox"
                                        className="checkbox"
                                        onClick={() => {
                                            if (chosenLocations.has(location)) {
                                                if (chosenLocations.size !== 0) {
                                                    chosenLocations.delete(location);
                                                    updateResources();
                                                    var allBox = document.getElementById("allLocations");
                                                    if (
                                                        allBox != null &&
                                                        allBox instanceof HTMLInputElement &&
                                                        allBox.checked
                                                    ) {
                                                        allBox.checked = false;
                                                    }
                                                }
                                            } else {
                                                chosenLocations.add(location);
                                                updateResources();
                                            }
                                        }}
                                    />
                                </label>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>

            <div className="dropdown dropdown-hover">
                <label tabIndex={0} className="btn m-1">
                    Buildings
                </label>
                <div className="form-control">
                    <ul
                        tabIndex={0}
                        className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
                    >
                        <li key="li_allBuildings">
                            <label className="label cursor-pointer">
                                <span className="label-text">All Buildings</span>
                                <input
                                    id="allBuildings"
                                    type="checkbox"
                                    className="checkbox"
                                    onClick={() => {
                                        buildings.forEach((building) => {
                                            var ebox = document.getElementById(building);

                                            if (ebox != null && ebox instanceof HTMLInputElement) {
                                                var box: HTMLInputElement = ebox;
                                                if (!box.checked) {
                                                    chosenLocations.add(building);
                                                }
                                                box.checked = true;
                                            }

                                        });
                                        updateResources();
                                    }}
                                />
                            </label>
                        </li>
                        <div className="divider"></div>

                        {Array.from(buildings).map((building: string) => (
                            <li key={building}>
                                <label className="label cursor-pointer">
                                    <span className="label-text">{building}</span>
                                    <input
                                        id={building}
                                        type="checkbox"
                                        className="checkbox"
                                        onClick={() => {
                                            if (chosenBuildings.has(building)) {
                                                if (chosenBuildings.size !== 0) {
                                                    chosenBuildings.delete(building);
                                                    updateResources();
                                                    var allBox = document.getElementById("allBuildings");
                                                    if (
                                                        allBox != null &&
                                                        allBox instanceof HTMLInputElement &&
                                                        allBox.checked
                                                    ) {
                                                        allBox.checked = false;
                                                    }
                                                }
                                            } else {
                                                chosenBuildings.add(building);
                                                updateResources();
                                            }
                                        }}
                                    />
                                </label>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>

            <div className="dropdown dropdown-hover">
                <label tabIndex={0} className="btn m-1">
                    Rooms
                </label>
                <div className="form-control">
                    <ul
                        tabIndex={0}
                        className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
                    >
                        <li key="li_allRooms">
                            <label className="label cursor-pointer">
                                <span className="label-text">All Rooms</span>
                                <input
                                    id="allRooms"
                                    type="checkbox"
                                    className="checkbox"
                                    onClick={() => {
                                        rooms.forEach((room) => {
                                            var ebox = document.getElementById(room);

                                            if (ebox != null && ebox instanceof HTMLInputElement) {
                                                var box: HTMLInputElement = ebox;
                                                if (!box.checked) {
                                                    chosenLocations.add(room);
                                                    updateResources();
                                                }
                                                box.checked = true;
                                            }
                                        });
                                        updateResources();
                                    }}
                                />
                            </label>
                        </li>
                        <div className="divider"></div>

                        {Array.from(rooms).map((room: string) => (
                            <li key={room}>
                                <label className="label cursor-pointer">
                                    <span className="label-text">{room}</span>
                                    <input
                                        id={room}
                                        type="checkbox"
                                        className="checkbox"
                                        onClick={() => {
                                            if (chosenRooms.has(room)) {
                                                if (chosenRooms.size !== 0) {
                                                    chosenRooms.delete(room);
                                                    updateResources();
                                                    var allBox = document.getElementById("allRooms");
                                                    if (
                                                        allBox != null &&
                                                        allBox instanceof HTMLInputElement &&
                                                        allBox.checked
                                                    ) {
                                                        allBox.checked = false;
                                                    }
                                                }
                                            } else {
                                                chosenRooms.add(room);
                                                updateResources();
                                            }
                                        }}
                                    />
                                </label>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>


            <div className="flex items-center justify-between">
                <h1 className="text-3xl font-bold text-left my-10">Resources Overview</h1>
                <button type="button" className="btn btn-secondary bg-deskstar-green-dark hover:bg-deskstar-green-light border-deskstar-green-dark hover:border-deskstar-green-light" onClick={() => { }}>
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
            users: users.filter((user: IUser) => user.isApproved),
        },
    };
};
