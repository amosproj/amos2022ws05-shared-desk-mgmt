import { IDesk } from "../types/desk";
import React, { useState } from "react";
import { time } from "console";

export const DropDownFilter = ({ title, options, onSelectionChanged }: { title: string, options: string[], onSelectionChanged: (selection: string[]) => any }) => {
    const [optionsSet, _] = useState(new Set<string>(options));
    const [selectedOptions, __] = useState(new Set<string>());

    const onSingleSelectionClicked = (option: string) => {
        var allBox = document.getElementById(`all${title}`);

        if (!selectedOptions.has(option)) {
            selectedOptions.add(option);

            // set all checkbox to true if all others are checked
            if (selectedOptions.size === optionsSet.size) {
                if (allBox != null && allBox instanceof HTMLInputElement) {
                    allBox.checked = true;
                }
            }

            onSelectionChanged(Array.from(selectedOptions));
            return;
        }

        if (selectedOptions.size !== 0) {
            selectedOptions.delete(option);

            // reset all checkbox if not all others are checked
            if (allBox != null && allBox instanceof HTMLInputElement) {
                allBox.checked = false;
            }
            onSelectionChanged(Array.from(selectedOptions));
            return;
        }
    }

    const onAllSelectionClicked = () => {
        let allBox = document.getElementById(`all${title}`);
        let newState = true;
        if (allBox != null && allBox instanceof HTMLInputElement && !allBox.checked) {
            newState = false;
        }

        let newSelection = newState === true ? Array.from(optionsSet) : [];
        selectedOptions.clear();
        newSelection.forEach((s) => selectedOptions.add(s));

        optionsSet.forEach((option) => {
            let ebox = document.getElementById(option);
            if (ebox != null && ebox instanceof HTMLInputElement) {
                ebox.checked = newState;
            }
        });

        onSelectionChanged(newSelection);
    }

    const allLocationsCheckbox = <li key={`li_all${title}`}>
        <label className="label cursor-pointer">
            <span className="label-text">All {title}</span>
            <input
                id={`all${title}`}
                type="checkbox"
                className="checkbox"
                onClick={onAllSelectionClicked}
            />
        </label>
    </li>;

    return (
        <div className="dropdown dropdown-hover">
            <label tabIndex={0} className="btn m-1">{title}</label>
            <div className="form-control">
                <ul tabIndex={0} className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52">
                    {allLocationsCheckbox}
                    <div className="divider"></div>
                    {Array.from(optionsSet).map((option: string) => (
                        <li key={`li_${option}`}>
                            <DropDownFilterEntry option={option} onClick={onSingleSelectionClicked} />
                        </li>
                    ))}
                </ul>
            </div>
        </div>

    );
}

const DropDownFilterEntry = ({ option, onClick }: { option: string, onClick: (option: string) => any }) => {
    return <label className="label cursor-pointer">
        <span className="label-text">{option}</span>
        <input
            id={option}
            type="checkbox"
            className="checkbox"
            onClick={() => { onClick(option); }}
        />
    </label>

}