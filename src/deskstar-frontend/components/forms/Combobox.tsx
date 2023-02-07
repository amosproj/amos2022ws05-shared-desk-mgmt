import { Combobox } from "@headlessui/react";
import { useState } from "react";

export default function OwnCombobox<T>({
  entities,
  getName,
  selected,
  setSelected,
}: {
  entities: (T & { id: string })[];
  getName: (entity: T | null) => string;
  selected: T;
  setSelected: (selected: T) => void;
}) {
  const [query, setQuery] = useState("");

  const filteredEntities = entities.filter((entity) =>
    getName(entity).toLowerCase().includes(query.toLowerCase())
  );

  return (
    <div className="dropdown my-2">
      <Combobox value={selected} onChange={setSelected} nullable>
        <p>Your Company</p>
        <Combobox.Input
          className="input w-full border-2 border-gray-300 p-2 rounded-lg"
          placeholder="Company"
          onChange={(event) => setQuery(event.target.value)}
          displayValue={getName}
        />
        <Combobox.Options className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52">
          {filteredEntities.map((entity) => (
            <Combobox.Option key={entity.id} value={entity}>
              {({ active }) => (
                <a className={active ? "active" : ""}>{getName(entity)}</a>
              )}
            </Combobox.Option>
          ))}
        </Combobox.Options>
      </Combobox>
    </div>
  );
}
