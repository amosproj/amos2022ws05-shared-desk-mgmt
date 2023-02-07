import { Dialog } from "@headlessui/react";

export default function GenericAddResourceModal({
  isOpen,
  setIsOpen,
  isLoading,
  setIsLoading,
  actionAdd,
  title,
  children,
}: {
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
  isLoading: boolean;
  setIsLoading: (isLoading: boolean) => void;
  actionAdd: () => Promise<void>;
  title: string;
  children: React.ReactNode;
}) {
  return (
    <Dialog open={isOpen} onClose={() => setIsOpen(false)}>
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4 ">
        <Dialog.Panel
          className={"card bg-white dark:bg-gray-300 dark:text-black w-96"}
        >
          <div className="card-body">
            <Dialog.Title as="div" className="card-title flex justify-between">
              <div>
                <p>{title}</p>
              </div>
            </Dialog.Title>
            <Dialog.Description as="div" className={"py-4"}>
              <div>
                {children}
                <div className="card-actions justify-end mt-4">
                  <button
                    disabled={isLoading}
                    className="btn btn-primary"
                    onClick={async () => {
                      setIsLoading(true);
                      await actionAdd();

                      setIsOpen(false);
                      setIsLoading(false);
                    }}
                  >
                    Confirm
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
