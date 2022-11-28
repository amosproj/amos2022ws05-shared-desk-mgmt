import { signOut, useSession } from "next-auth/react";
import Link from "next/link";
import { useState } from "react";
import { CloseIcon, HamburgerIcon } from "./Icons";
import deskstarLogo from "assets/img/team-logo.png";
import Image from "next/image";

const userNavItems = [
  {
    name: "Home",
    href: "/",
  },
  {
    name: "Bookings",
    href: "/bookings",
  },
  {
    name: "Resources",
    href: "/searchResults",
  },
  {
    name: "Add New Booking",
    href: "/addBooking",
  },
];

const adminNavItems = [
  {
    name: "User Management",
    href: "/usersOverview",
  },
];

export default function Navbar() {
  const [isOpen, setIsOpen] = useState(false);

  const { data: session } = useSession();

  return (
    <nav className="flex flex-row justify-between p-4 py-3 rounded bg-deskstar-green-dark dark:text-black">
      <Link href="/">
        <Image src={deskstarLogo} alt="Deskstar Logo" width={50} height={50} />
        <span className="sr-only">Deskstar</span>
      </Link>
      {/* Desktop menu, so hidden if smaller than md */}
      <div className="hidden md:flex flex-row">
        {userNavItems.map((item) => (
          <Link
            className="flex items-center text-lg mx-2 p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
            href={item.href}
            key={item.name}
          >
            {item.name}
          </Link>
        ))}

        {session &&
          session.user &&
          session.user.isAdmin &&
          adminNavItems.map((item) => (
            <Link
              className="flex items-center text-lg mx-2 p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
              href={item.href}
              key={item.name}
            >
              {item.name}
            </Link>
          ))}
        {session && (
          <span
            className="flex items-center text-lg mx-2 p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
            onClick={() => signOut()}
          >
            Logout
          </span>
        )}
      </div>

      {/* Mobile menu */}
      <div className="md:hidden">
        <div
          onClick={() => setIsOpen(true)}
          className="flex h-full items-center cursor-pointer"
        >
          <HamburgerIcon className="w-8 h-8" />
        </div>

        {isOpen && (
          <div className="absolute top-0 right-0 m-2 bg-deskstar-green-dark rounded">
            <div className="flex flex-col items-end gap-1 p-2 min-w-[50vw]">
              <span
                onClick={() => setIsOpen(false)}
                className="flex items-center text-lg p-2 py-1 pt-3 rounded cursor-pointer hover:bg-deskstar-green-light"
              >
                <CloseIcon className="w-8 h-8" />
              </span>
              {userNavItems.map((item) => (
                <Link
                  href={item.href}
                  key={item.name}
                  onClick={() => setIsOpen(false)}
                  className="w-full text-lg text-right p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
                >
                  {item.name}
                </Link>
              ))}
              {session &&
                session.user &&
                session.user.isAdmin &&
                adminNavItems.map((item) => (
                  <Link
                    href={item.href}
                    key={item.name}
                    onClick={() => setIsOpen(false)}
                    className="w-full text-lg text-right p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
                  >
                    {item.name}
                  </Link>
                ))}

              {session && (
                <span onClick={() => signOut()}>
                  <a
                    onClick={() => setIsOpen(false)}
                    className="w-full text-lg text-right p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
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
