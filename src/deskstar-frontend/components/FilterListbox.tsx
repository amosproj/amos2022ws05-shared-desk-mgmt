import { Listbox } from "@headlessui/react";
import { Fragment } from "react";
import { classes } from "../lib/helpers";

function compareItems<A>(
  getKey: (item: A) => string
): (a: A | null, b: A | null) => boolean {
  return (a, b) => {
    if (a === null && b === null) {
      return true;
    }

    if (a === null || b === null) {
      return false;
    }

    return getKey(a) === getKey(b);
  };
}

type FilterListboxProps<A> = {
  items: A[];
  selectedItem: A | null;
  setSelectedItem: (item: A | null) => void;
  getKey?: (item: A) => string;
  getName: (item: A | null) => string;
  allOption?: boolean;
};

export default function FilterListbox<A>({
  items,
  selectedItem,
  setSelectedItem,
  getName,
  getKey,
  allOption = false,
}: FilterListboxProps<A>) {
  return (
    <div>
      <Listbox
        value={selectedItem}
        by={getKey ? compareItems(getKey) : undefined}
        onChange={setSelectedItem}
      >
        <Listbox.Button className="btn">
          {!allOption || selectedItem ? getName(selectedItem) : "All"}
        </Listbox.Button>
        <Listbox.Options
          as="ul"
          className="absolute menu bg-base-300 shadow p-2 rounded-box z-50"
        >
          {allOption && (
            <Listbox.Option key="all" value={null} as="li">
              <a>All</a>
            </Listbox.Option>
          )}

          {items.map((item) => (
            <Listbox.Option
              key={getKey ? getKey(item) : getName(item)}
              value={item}
              as="li"
            >
              <a>{getName(item)}</a>
            </Listbox.Option>
          ))}
        </Listbox.Options>
      </Listbox>
    </div>
  );
}
