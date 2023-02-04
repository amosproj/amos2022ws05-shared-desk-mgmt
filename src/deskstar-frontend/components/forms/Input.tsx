import { classes } from "../../lib/helpers";

type InputProps = {
  name: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  value?: string;
  checked?: boolean;
  className?: string;
  placeholder?: string;
  type?: string;
};

export default function Input({
  name,
  value,
  checked,
  onChange,
  className,
  placeholder,
  type,
}: InputProps) {
  if (type === "checkbox")
    return (
      <div className="py-2 flex">
        <input
          name={name}
          checked={checked ?? false}
          onChange={onChange}
          type={"checkbox"}
          className={classes("checkbox", className)}
        />
        <label htmlFor={name} className="ml-5 items-center">
          {name}
        </label>
      </div>
    );
  return (
    <div className="py-2">
      <label htmlFor={name}>{name}</label>
      <input
        name={name}
        value={value ?? ""}
        onChange={onChange}
        type={type || "text"}
        placeholder={placeholder}
        className={classes(
          className,
          "dark:bg-base-100 w-full border-2 border-gray-300 p-2 rounded-lg"
        )}
      />
    </div>
  );
}
