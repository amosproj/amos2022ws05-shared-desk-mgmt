import { classes } from "../../lib/helpers";

type InputProps = {
  name: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  value: string;
  className?: string;
  placeholder?: string;
  type?: string;
};

export default function Input({
  name,
  value,
  onChange,
  className,
  placeholder,
  type,
}: InputProps) {
  return (
    <div className="py-2">
      <label htmlFor={name}>{placeholder}</label>
      <input
        name={name}
        value={value}
        onChange={onChange}
        type={type || "text"}
        placeholder={placeholder}
        className={classes(
          className,
          "w-full border-2 border-gray-300 p-2 rounded-lg"
        )}
      />
    </div>
  );
}
