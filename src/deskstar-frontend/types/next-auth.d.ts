import NextAuth, { DefaultSession } from "next-auth";

declare module "next-auth" {
  /**
   * Returned by `useSession`, `getSession` and received as a prop on the `SessionProvider` React Context
   */
  interface Session {
    user: {
      id: string;
      accessToken: string;
      isApproved: boolean;
      isAdmin: boolean;
    } & DefaultSession["user"];
    accessToken: string;
  }

  interface User {
    our_token?: string;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken: string;
  }
}
