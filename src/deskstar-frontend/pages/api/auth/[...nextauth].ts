import NextAuth from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import { AuthError, authorize } from "../../../lib/api/AuthService";

export const authOptions = {
  secret: process.env.SECRET,
  // Configure one or more authentication providers
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        company: {
          label: "Company",
          type: "text ",
          placeholder: "INTERFLEX",
          value: "INTERFLEX",
        },
        email: { label: "E-Mailadresse", type: "text" },
        password: { label: "Passwort", type: "password" },
      },
      async authorize(credentials, req) {
        // Check if credentials contains an email and password
        if (
          !credentials ||
          !credentials.company ||
          !credentials.email ||
          !credentials.password
        ) {
          return null;
        }

        const result = await authorize(credentials.email, credentials.password);
        if (!(result instanceof String)) {
          // must be AuthError
          const err = result as AuthError;

          return null;
        }

        const user = {
          id: "1",
          company: "INTERFLEX",
          name: "testuser",
          email: "test@example.com",
          token: result as String,
        };

        if (user) {
          return user;
        } else {
          return null;
        }
      },
    }),
    // ...add more providers here
  ],
  pages: {
    signIn: "/login",
    newUser: "/register",
  },
};

export default NextAuth(authOptions);
