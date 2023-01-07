import NextAuth, { Awaitable, NextAuthOptions, User } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import { userAgent } from "next/server";
import { AuthResponse, authorize } from "../../../lib/api/AuthService";
import { Session } from "next-auth";
import { getUserMe } from "../../../lib/api/UserService";

export const authOptions: NextAuthOptions = {
  secret: process.env.SECRET,
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        email: { label: "E-Mailadresse", type: "text" },
        password: { label: "Passwort", type: "password" },
      },
      async authorize(credentials, req): Promise<User | null> {
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

        let accessToken = result as string;

        const userResponse = await getUserMe(accessToken);

        if (!userResponse) {
          return null;
        }

        const user: User = {
          id: userResponse.userId,
          company: userResponse.company,
          name: userResponse.firstName + " " + userResponse.lastName,
          email: userResponse.mailAddress,
          isApproved: userResponse.isApproved,
          isAdmin: userResponse.isCompanyAdmin,
          our_token: accessToken,
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
    async signIn({ user, account, profile }) {
      return user.isApproved;
    },
    async jwt({ token, user, account, isNewUser }) {
      if (user) {
        if (user.our_token) {
          token = { accessToken: user.our_token };
        }
      }
      return token;
    },
    async session({ session, token, user }) {
      session.accessToken = token.accessToken;

      // There is a cache definition in getUserMe to prevent overloading the backend
      const userResponse = await getUserMe(token.accessToken);
      if (!userResponse) {
        return session;
      }

      session.user = {
        id: userResponse.userId,
        company: userResponse.company,
        name: userResponse.firstName + " " + userResponse.lastName,
        email: userResponse.mailAddress,
        isApproved: userResponse.isApproved,
        isAdmin: userResponse.isCompanyAdmin,
        accessToken: token.accessToken,
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
