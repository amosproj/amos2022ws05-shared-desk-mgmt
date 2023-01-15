import React, { useEffect, useState } from "react";

function getUniqueArray<A>(array: A[], getKey: (item: A) => string): A[] {
  return array.filter((item, index) => {
    return array.findIndex((item2) => getKey(item) === getKey(item2)) === index;
  });
}

export default function DropDownFilter<A>({
  title,
  options,
  getItemName,
  setSelectedOptions: parentSetSelectedOptions,
}: {
  title: string;
  getItemName: (item: A) => string;
  options: A[];
  setSelectedOptions: (newSelectedOptions: A[]) => void;
}) {
  options = getUniqueArray(options, getItemName);
  const [allChecked, setAllChecked] = useState(false);
  const [selectedOptions, _setSelectedOptions] = useState<A[]>([]);

  function setSelectedOptions(newSelectedOptions: A[]) {
    _setSelectedOptions(newSelectedOptions);
    parentSetSelectedOptions(newSelectedOptions);
  }

  const onSingleSelected = (option: A, selected: boolean) => {
    let newSelectedOptions = selected
      ? [...selectedOptions, option]
      : selectedOptions.filter((o) => getItemName(o) != getItemName(option));

    // remove duplicates in newSelectedOptions
    newSelectedOptions = newSelectedOptions.filter(
      (o, i) => newSelectedOptions.indexOf(o) === i
    );

    setAllChecked(newSelectedOptions.length === options.length);

    setSelectedOptions(newSelectedOptions);
  };

  useEffect(() => {
    setAllChecked(selectedOptions.length === options.length);
  }, [setAllChecked, selectedOptions, options]);

  return (
    <div className="dropdown dropdown-hover">
      <label tabIndex={0} className="btn mr-1">
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

                  options.forEach((_, index) => {
                    const checkbox = document.getElementById(
                      `${title}_checkbox_${index}`
                    ) as HTMLInputElement;

                    if (checkbox) {
                      checkbox.checked = e.target.checked;
                    }
                  });

                  setSelectedOptions(e.target.checked ? options : []);
                }}
              />
            </label>
          </li>

          {/* Single Selection Checkboxes */}
          <div className="divider"></div>
          {options &&
            options.map((option, index) => {
              return (
                <li key={`${title}_checkbox_${index}`}>
                  <DropDownFilterEntry
                    name={getItemName(option)}
                    id={`${title}_checkbox_${index}`}
                    option={option}
                    defaultChecked={selectedOptions.includes(option)}
                    setSelected={(selected) => {
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
  id,
  option,
  defaultChecked,
  setSelected,
}: {
  name: string;
  id: string;
  option: A;
  defaultChecked: boolean;
  setSelected: (checked: boolean) => void;
}) {
  return (
    <label className="label cursor-pointer">
      <span className="label-text">{name}</span>
      <input
        id={id}
        type="checkbox"
        className="checkbox"
        defaultChecked={defaultChecked}
        onChange={(e) => setSelected(e.target.checked)}
      />
    </label>
  );
}
