import Link from "next/link";

const navItems = [
  {
    name: "Home",
    href: "/",
  },
  {
    name: "Bookings",
    href: "/bookings",
  },
  {
    name: "Login",
    href: "/login",
  },
];

export default function Navbar() {
  return (
    <nav className="flex flex-row justify-between p-4 py-3 rounded bg-slate-400">
      <span className="text-xl flex items-center">Deskstar</span>
      <div className="flex flex-row">
        {navItems.map((item) => (
          <Link href={item.href} key={item.name}>
            <a className="flex items-center text-lg mx-2 p-2 py-1 rounded hover:text-white hover:bg-slate-500">
              {item.name}
            </a>
          </Link>
        ))}
      </div>
    </nav>
  );
}
