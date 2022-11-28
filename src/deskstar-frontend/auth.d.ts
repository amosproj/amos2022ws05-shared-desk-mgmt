import "next-auth";

declare module "next-auth" {
  interface User {
    id: String;
    email: string;
    name: string;
    isApproved: boolean;
    isAdmin: boolean;
  }

  interface Session {
    user: User;
  }
}