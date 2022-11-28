import NextAuth, { NextAuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import { userAgent } from "next/server";
import { AuthResponse, authorize } from "../../../lib/api/AuthService";

export const authOptions: NextAuthOptions = {
  secret: process.env.SECRET,
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        email: { label: "E-Mailadresse", type: "text" },
        password: { label: "Passwort", type: "password" },
      },
      async authorize(credentials, req) {
        // Check if credentials contains an email and password
        if (!credentials || !credentials.email || !credentials.password) {
          return null;
        }

        const result = await authorize(credentials.email, credentials.password);

        console.log(result);

        if (typeof result !== "string") {
          // must be AuthError
          const err = result as AuthResponse;

          throw Error(AuthResponse[err]);
        }

        const user = {
          id: "1",
          company: "INTERFLEX",
          name: "testuser",
          email: "test@example.com",
          our_token: result as String,
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
  callbacks: {
    async jwt({ token, user, account, isNewUser }) {
      if (user) {
        if (user.our_token) {
          token = { accessToken: user.our_token };
        }
      }
      return token;
    },
    async session({ session, token }) {
      session.accessToken = token.accessToken as string;

      // TODO: Needs to be fetched via /user/me route or so!
      session.user = {
        id: "1",
        name: "testuser",
        email: "test@example.com",
        accessToken: token.accessToken as string,
      };
      return session;
    },
  },
  pages: {
    signIn: "/login",
    newUser: "/register",
  },
};

export default NextAuth(authOptions);
