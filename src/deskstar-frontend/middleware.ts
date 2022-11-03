export { default } from "next-auth/middleware";

// Trigger this middleware to run on the `/secret-page` route
export const config = {
  matcher: "/app/:path*",
};
