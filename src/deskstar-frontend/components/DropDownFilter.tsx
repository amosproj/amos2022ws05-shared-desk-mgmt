import React, { useState } from "react";

interface Selectable {
  getName: () => string;
}

export function stringToSelectable(list: string[]): Selectable[] {
  return list.map((x) => ({
    getName() {
      return x;
    },
  }));
}

export default function DropDownFilter({
  title,
  options,
  setSelectedOptions,
}: {
  title: string;
  options: Selectable[];
  setSelectedOptions: (newSelectedOptions: Selectable[]) => void;
}) {
  const [selectedOptions, _setSelectedOptions] = useState<Selectable[]>([]);

  let [allChecked, setAllChecked] = useState(false);

  function selectOptions(selectedOptions: Selectable[]) {
    _setSelectedOptions(selectedOptions);
    setSelectedOptions(selectedOptions);

    // check if newSelectedOptions is equal to options
    // If the length is not equal, then not all options are selected
    setAllChecked(selectedOptions.length === options.length);
  }

  const onSingleSelected = (option: Selectable, selected: boolean) => {
    const newSelectedOptions = selected
      ? [...selectedOptions, option]
      : selectedOptions.filter((o) => o !== option);

    selectOptions(newSelectedOptions);
  };

  return (
    <div className="dropdown dropdown-hover">
      <label tabIndex={0} className="btn m-1">
        {title}
      </label>
      <div className="form-control">
        <ul
          tabIndex={0}
          className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
        >
          {/* All Selection Checkbox */}
          <li key={`li_all${title}`}>
            <label className="label cursor-pointer">
              <span className="label-text">All {title}</span>
              <input
                id={`all${title}`}
                type="checkbox"
                className="checkbox"
                checked={allChecked}
                onClick={() => {
                  if (!allChecked) {
                    selectOptions(options);
                  } else {
                    selectOptions([]);
                  }
                }}
              />
            </label>
          </li>

          {/* Single Selection Checkboxes */}
          <div className="divider"></div>
          {Array.from(options).map((option: Selectable) => (
            <li key={option.getName()}>
              <DropDownFilterEntry
                option={option}
                selected={selectedOptions.includes(option)}
                setSelected={(selected) => {
                  onSingleSelected(option, selected);
                }}
              />
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}

const DropDownFilterEntry = ({
  option,
  selected,
  setSelected,
}: {
  option: Selectable;
  selected: boolean;
  setSelected: (checked: boolean) => void;
}) => {
  return (
    <label className="label cursor-pointer">
      <span className="label-text">{option.getName()}</span>
      <input
        id={option.getName()}
        type="checkbox"
        className="checkbox"
        checked={selected}
        onClick={() => {
          setSelected(!selected);
        }}
      />
    </label>
  );
};
