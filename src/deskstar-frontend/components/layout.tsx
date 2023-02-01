import { useSession } from "next-auth/react";
import Sidebar from "./Sidebar";
import { ToastContainer } from "./ToastContainer";

type LayoutProps = {
  children: React.ReactNode;
};

export default function Layout({ children }: LayoutProps) {
  const { data: session } = useSession();

  if (!session)
    return (
      <>
        <ToastContainer />
        <div className="container mx-auto p-2 md:p-4 min-h-[100vh] flex flex-col justify-between">
          <main className="flex-1 py-2">{children}</main>
          <footer className="text-center">
            {/* No copyright, because MIT License */}
            Deskstar {new Date().getFullYear()}
          </footer>
        </div>
      </>
    );

  return (
    <>
      <ToastContainer />
      <Sidebar>
        <div className="container p-2 md:p-4 min-h-[100vh] flex flex-col justify-between">
          <main className="flex-1 py-2">{children}</main>
          <footer className="text-center">
            {/* No copyright, because MIT License */}
            Deskstar {new Date().getFullYear()}
          </footer>
        </div>
      </Sidebar>
    </>
  );
}
