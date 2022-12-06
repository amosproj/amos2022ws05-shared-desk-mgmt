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
    subNavItems: [
      {
        name: "Overview",
        href: "/users",
      },
      {
        name: "Requests",
        href: "/users/requests",
      },
    ],
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
            adminNavItems.map((item) => {
              if ("subNavItems" in item)
                return (
                  <CollapseSideBarEntry
                    key={item.name}
                    href={item.href}
                    name={item.name}
                    subNavItems={item.subNavItems}
                  />
                );
              else
                return (
                  <SidebarEntry
                    key={item.name}
                    href={item.href}
                    name={item.name}
                  />
                );
            })}
          <li>
            <div onClick={() => signOut()} className="dark:text-black">Logout</div>
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
      <h1 className="text-3xl ml-10 leading-normal dark:text-black">Deskstar</h1>
    </div>
  );
};

type SidebarEntryProps = {
  href: string;
  name: string;
  subNavItems?: SidebarEntryProps[];
};

const SidebarEntry = ({ href, name }: SidebarEntryProps) => {
  // close mobile drawer on click
  const closeSidebar = () => document.getElementById("my-drawer")?.click();
  return (
    <li onClick={closeSidebar}>
      <Link href={href} className="dark:text-black">{name}</Link>
    </li>
  );
};

const CollapseSideBarEntry = ({
  href,
  name,
  subNavItems,
}: SidebarEntryProps) => {
  return (
    <div tabIndex={0} className="collapse collapse-arrow">
      <input type="checkbox" />
      <div className="collapse-title dark:text-black">{name}</div>
      <div className="collapse-content">
        {subNavItems?.map((subItem) => (
          <SidebarEntry
            key={subItem.name}
            href={subItem.href}
            name={subItem.name}
          />
        ))}
      </div>
    </div>
  );
};
