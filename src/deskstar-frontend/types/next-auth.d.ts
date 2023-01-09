import NextAuth, { DefaultSession } from "next-auth";

declare module "next-auth" {
  /**
   * Returned by `useSession`, `getSession` and received as a prop on the `SessionProvider` React Context
   */
  interface Session {
    user: {
      id: String;
      accessToken: String;
      isApproved: boolean;
      isAdmin: boolean;
      company: {
        companyName: String;
        companyId: String;
      };
    } & DefaultSession["user"];
    accessToken: String;
  }

  interface User {
    our_token?: String;
    company: {
      companyName: String;
      companyId: String;
    };
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken: String;
  }
}
