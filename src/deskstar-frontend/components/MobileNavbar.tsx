import { HamburgerIcon } from "./Icons";

export default function MobileNavbar() {
  return (
    <div className="w-full navbar bg-primary lg:hidden">
      <div className="flex-none lg:hidden">
        <label htmlFor="my-drawer" className="btn btn-square btn-ghost">
          <HamburgerIcon />
        </label>
      </div>
      <div className="flex-1 px-2 mx-2">Deskstar</div>
    </div>
  );
}
