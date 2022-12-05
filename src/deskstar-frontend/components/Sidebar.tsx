import React from "react";
import Link from "next/link";
import { signOut, useSession } from "next-auth/react";
import Image from "next/image";
import deskstarLogo from "assets/img/team-logo.png";
import MobileNavbar from "./MobileNavbar";

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
    href: "/bookings/add",
  },
];

const adminNavItems = [
  {
    name: "User Management",
    href: "/users",
  },
  {
    name: "Resource Management",
    href: "/resources",
  },
];

type SidebarProps = {
  children: React.ReactNode;
};

export default function Sidebar({ children }: SidebarProps) {
  const { data: session } = useSession();

  return (
    <div className="drawer drawer-mobile">
      <input id="my-drawer" type="checkbox" className="drawer-toggle" />
      <div className="drawer-content">
        {/* Page content here */}
        <MobileNavbar />
        {children}
      </div>

      <div className="drawer-side ">
        <label htmlFor="my-drawer" className="drawer-overlay"></label>
        <ul className="menu p-4 w-80 bg-base-100 text-base-content bg-deskstar-green-dark">
          {/* Sidebar content here */}
          <SidebarHeader />
          {userNavItems.map((item) => (
            <SidebarEntry key={item.name} href={item.href} name={item.name} />
          ))}
          {session &&
            session.user &&
            session.user.isAdmin &&
            adminNavItems.map((item) => (
              <SidebarEntry key={item.name} href={item.href} name={item.name} />
            ))}
          <li>
            <div onClick={() => signOut()}>Logout</div>
          </li>
        </ul>
      </div>
    </div>
  );
}

const SidebarHeader = () => {
  return (
    <div className="flex flex-row justify-start mb-5">
      <Link href="/">
        <Image src={deskstarLogo} alt="Deskstar Logo" width={50} height={50} />
        <span className="sr-only">Deskstar</span>
      </Link>
      <h1 className="text-3xl ml-10 leading-normal">Deskstar</h1>
    </div>
  );
};

type SidebarEntryProps = {
  href: string;
  name: string;
};

const SidebarEntry = ({ href, name }: SidebarEntryProps) => {
  // close mobile drawer on click
  const closeSidebar = () => document.getElementById("my-drawer")?.click();
  return (
    <li onClick={closeSidebar}>
      <Link href={href}>{name}</Link>
    </li>
  );
};