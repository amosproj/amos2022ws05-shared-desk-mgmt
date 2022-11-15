export { default } from "next-auth/middleware";

export const config = {
  matcher: ["/((?!login|logout|api|register|favicon.ico).*)"],
};
