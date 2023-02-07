import React from "react";

const Collapse = ({
  index,
  title,
  children,
  defaultChecked,
}: {
  index: number;
  title: string;
  children: React.ReactNode;
  defaultChecked?: boolean;
}) => {
  return (
    <div
      tabIndex={index}
      className="collapse collapse-arrow shadow bg-base-100 rounded-box"
    >
      <input type="checkbox" defaultChecked={defaultChecked} />
      <div className="collapse-title text-xl font-medium">{title}</div>
      <div className="collapse-content flex flex-col gap-2 px-1 pl-2">
        {children}
      </div>
    </div>
  );
};

export default Collapse;
