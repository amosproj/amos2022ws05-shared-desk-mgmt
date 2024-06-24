import { FaPencilAlt, FaTrashAlt, FaTrashRestore } from "react-icons/fa";
import { Identifiable } from "../../types";

type ResourceTableProps<T> = {
  resources: (T & Identifiable)[];
  setResources: (resources: (T & Identifiable)[]) => void;
  filterResources?: (resources: (T & Identifiable)[]) => (T & Identifiable)[];
  headers: string[];
  fields: ((resource: T & Identifiable) => string)[];
  editable?: boolean;
  deletable?: boolean;
  restorable?: boolean;
};

export default function ResourceTable<T>({
  resources,
  setResources,
  filterResources,
  headers,
  fields,
  editable,
  deletable,
  restorable,
}: ResourceTableProps<T>) {
  const filterRes = filterResources ?? ((resources) => resources);
  return (
    <div className="overflow-x-auto table-auto">
      <table className="table table-zebra w-full">
        <thead className="dark:text-black">
          <tr>
            {headers.map((header) => (
              <th key={header} className="bg-secondary text-left">
                {header}
              </th>
            ))}

            {(deletable || editable) && <th className="bg-secondary"></th>}
            {restorable && <th className="bg-secondary text-right">Restore</th>}
          </tr>
        </thead>
        <tbody>
          {filterRes(resources).map((resource) => (
            <ResourceTableEntry
              key={resource.id}
              resource={resource}
              fields={fields}
              editable={editable}
              deletable={deletable}
              restorable={restorable}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
}

type ResourceTableEntryProps<T> = {
  resource: T & Identifiable;
  fields: ((resource: T & Identifiable) => string)[];
  editable?: boolean;
  deletable?: boolean;
  restorable?: boolean;
};

function ResourceTableEntry<T>({
  fields,
  resource,
  deletable,
  editable,
  restorable,
}: ResourceTableEntryProps<T>) {
  return (
    <tr className="hover">
      {fields.map((field, idx) => (
        <td key={idx} className={`text-left ${idx == 0 ? "font-bold" : ""}`}>
          {field(resource)}
        </td>
      ))}

      {(deletable || editable) && (
        <td className="p-0 pr-2 text-right">
          {editable && (
            <button className="btn btn-ghost" onClick={() => {}}>
              <FaPencilAlt />
            </button>
          )}
          {deletable && (
            <button className="btn btn-ghost" onClick={() => {}}>
              <FaTrashAlt color="red" />
            </button>
          )}
        </td>
      )}
      {restorable && (
        <td className="text-right">
          <button className="btn btn-ghost" onClick={() => {}}>
            <FaTrashRestore color="green" />
          </button>
        </td>
      )}
    </tr>
  );
}
