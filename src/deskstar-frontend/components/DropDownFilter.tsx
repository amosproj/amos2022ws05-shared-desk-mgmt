import React, { useEffect, useState } from "react";

export default function DropDownFilter<A>({
  title,
  options,
  getItemName,
  selectedOptions,
  setSelectedOptions,
}: {
  title: string;
  getItemName: (item: A) => string;
  options: A[];
  selectedOptions: A[];
  setSelectedOptions: (newSelectedOptions: A[]) => void;
}) {
  let [allChecked, setAllChecked] = useState(false);

  const onSingleSelected = (option: A, selected: boolean) => {
    let newSelectedOptions = selected
      ? [...selectedOptions, option]
      : selectedOptions.filter((o) => o !== option);

    // remove duplicates in newSelectedOptions
    newSelectedOptions = newSelectedOptions.filter(
      (o, i) => newSelectedOptions.indexOf(o) === i
    );

    console.log(newSelectedOptions);

    setSelectedOptions(newSelectedOptions);
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
                onChange={(e) => {
                  setAllChecked(e.target.checked);
                }}
              />
            </label>
          </li>

          {/* Single Selection Checkboxes */}
          <div className="divider"></div>
          {options &&
            options.map((option) => {
              return (
                <li key={getItemName(option)}>
                  <DropDownFilterEntry
                    name={getItemName(option)}
                    option={option}
                    selected={selectedOptions.includes(option)}
                    setSelected={(selected) => {
                      console.log(selected);
                      onSingleSelected(option, selected);
                    }}
                  />
                </li>
              );
            })}
        </ul>
      </div>
    </div>
  );
}

function DropDownFilterEntry<A>({
  name,
  option,
  selected,
  setSelected,
}: {
  name: string;
  option: A;
  selected: boolean;
  setSelected: (checked: boolean) => void;
}) {
  return (
    <label className="label cursor-pointer">
      <span className="label-text">{name}</span>
      <input
        id={name}
        type="checkbox"
        className="checkbox"
        checked={selected}
        onChange={(e) => setSelected(e.target.checked)}
      />
    </label>
  );
}
