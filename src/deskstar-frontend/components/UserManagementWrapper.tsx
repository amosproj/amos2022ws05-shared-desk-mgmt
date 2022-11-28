import Link from "next/link";

type LayoutProps = {
  children: React.ReactNode;
};

export function UserManagementWrapper({ children }: LayoutProps) {
  const navItems = [
    {
      name: "Overview",
      href: "/usersOverview",
    },
    {
      name: "Requests",
      href: "/userRequests",
    },
  ];

  return (
    <>
      <nav>
        {navItems.map((item) => (
          <Link
            className="flex items-center text-lg mx-2 p-2 py-1 rounded cursor-pointer hover:bg-deskstar-green-light"
            href={item.href}
            key={item.name}
          >
            {item.name}
          </Link>
        ))}
      </nav>
      {children}
    </>
  );
}
