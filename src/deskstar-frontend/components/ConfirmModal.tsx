import { useState } from "react";
import { Dialog } from "@headlessui/react";
import React from "react";

export default function ConfirmModal({
  title,
  description,
  text,
  warn,
  buttonText,
  action,
  isOpen,
  setIsOpen,
}: {
  title: string;
  description: string;
  text: string;
  warn: boolean;
  buttonText: string;
  action: () => void;
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
}) {
  let buttonClass = "btn ";
  if (warn) buttonClass += "btn-warning";

  return (
    <Dialog open={isOpen} onClose={() => setIsOpen(false)}>
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4 ">
        <Dialog.Panel
          className={"card bg-white dark:bg-gray-300 dark:text-black w-96"}
        >
          <div className="card-body">
            <Dialog.Title className="card-title flex justify-between">
              <div>
                <p>{title}</p>
                {warn && (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="stroke-current flex-shrink-0 h-10 w-10"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
                    />
                  </svg>
                )}
              </div>
            </Dialog.Title>
            <Dialog.Description className={"py-4"}>
              <div>
                <div>
                  <p>{description}</p>
                  <p>{text}</p>
                </div>
                <div className="card-actions justify-end mt-4">
                  <button
                    className={buttonClass}
                    onClick={() => {
                      setIsOpen(false);
                      action();
                    }}
                  >
                    {buttonText}
                  </button>

                  <button className="btn" onClick={() => setIsOpen(false)}>
                    Cancel
                  </button>
                </div>
              </div>
            </Dialog.Description>
          </div>
        </Dialog.Panel>
      </div>
    </Dialog>
  );
}
