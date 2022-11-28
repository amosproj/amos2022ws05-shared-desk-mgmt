import "next-auth";

declare module "next-auth" {
  interface User {
    id: String;
    email: string;
    name: string;
    isApproved: boolean;
    isCompanyAdmin: boolean;
  }

  interface Session {
    user: User;
  }
}