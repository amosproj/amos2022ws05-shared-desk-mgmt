import { signOut, useSession } from "next-auth/react";
import Link from "next/link";
import { useState } from "react";
import { CloseIcon, HamburgerIcon } from "./Icons";

const navItems = [
  {
    name: "Home",
    href: "/",
  },
  {
    name: "Bookings",
    href: "/bookings",
  },
];

export default function Navbar() {
  const [isOpen, setIsOpen] = useState(false);

  const { data: session } = useSession();

  return (
    <nav className="flex flex-row justify-between p-4 py-3 rounded bg-slate-400">
      <span className="text-xl flex items-center">Deskstar</span>
      {/* Desktop menu, so hidden if smaller than md */}
      <div className="hidden md:flex flex-row">
        {navItems.map((item) => (
          <Link href={item.href} key={item.name}>
            <a className="flex items-center text-lg mx-2 p-2 py-1 rounded cursor-pointer hover:text-white hover:bg-slate-500">
              {item.name}
            </a>
          </Link>
        ))}

        {session && (
          <span onClick={() => signOut()}>
            <a className="flex items-center text-lg mx-2 p-2 py-1 rounded cursor-pointer hover:text-white hover:bg-slate-500">
              Logout
            </a>
          </span>
        )}
      </div>

      {/* Mobile menu */}
      <div className="md:hidden">
        <div
          onClick={() => setIsOpen(true)}
          className="flex items-center cursor-pointer"
        >
          <HamburgerIcon className="w-8 h-8" />
        </div>

        {isOpen && (
          <div className="absolute top-0 right-0 m-2 bg-slate-400 rounded">
            <div className="flex flex-col items-end gap-1 p-2 min-w-[50vw]">
              <span
                onClick={() => setIsOpen(false)}
                className="flex items-center text-lg p-2 py-1 rounded cursor-pointer hover:text-white hover:bg-slate-500"
              >
                <CloseIcon className="w-8 h-8" />
              </span>
              {navItems.map((item) => (
                <Link href={item.href} key={item.name}>
                  <a
                    onClick={() => setIsOpen(false)}
                    className="w-full text-lg text-right p-2 py-1 rounded cursor-pointer hover:text-white hover:bg-slate-500"
                  >
                    {item.name}
                  </a>
                </Link>
              ))}

              {session && (
                <span onClick={() => signOut()}>
                  <a
                    onClick={() => setIsOpen(false)}
                    className="w-full text-lg text-right p-2 py-1 rounded cursor-pointer hover:text-white hover:bg-slate-500"
                  >
                    Logout
                  </a>
                </span>
              )}
            </div>
          </div>
        )}
      </div>
    </nav>
  );
}
