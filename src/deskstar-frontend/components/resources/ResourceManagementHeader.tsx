import { useRouter } from "next/router";
import Link from "next/link";

const resources = [
  {
    name: "Buildings",
    route: "/resources/buildings",
  },
  {
    name: "Floors",
    route: "/resources/floors",
  },

  {
    name: "Rooms",
    route: "/resources/rooms",
  },
  {
    name: "Desks",
    route: "/resources/desks",
  },
  {
    name: "Desk types",
    route: "/resources/desktypes",
  },
];

export default function ResourceManagementHeader({
  addAction,
}: {
  addAction?: () => void;
}) {
  // Get current route
  const router = useRouter();

  const currentResource = resources.find(
    (resource) => resource.route === router.pathname
  );

  return (
    <div className="flex items-center justify-between">
      <h1 className="text-3xl font-bold text-left mb-10 mt-5">
        {currentResource?.name}
      </h1>
      <div className="flex">
        <div className="dropdown dropdown-end">
          <label tabIndex={0} className="btn m-1">
            Resource: {currentResource?.name || "Select a resource"}
          </label>
          <ul
            tabIndex={0}
            className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
          >
            {resources.map((resource) => (
              <li key={resource.name}>
                <Link href={resource.route}>{resource.name}</Link>
              </li>
            ))}
          </ul>
        </div>

        {addAction && (
          <button onClick={addAction} className="btn btn-primary m-1">
            Add {currentResource?.name}
          </button>
        )}
      </div>
    </div>
  );
}
