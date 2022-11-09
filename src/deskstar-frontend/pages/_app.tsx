import "../styles/globals.css";
import { SessionProvider } from "next-auth/react";
import { Session } from "next-auth";
import type { AppProps } from "next/app";
import Layout from "../components/layout";

export default function App({
  Component,
  pageProps: { session, ...pageProps },
}: AppProps<{ session: Session }>) {
  return (
    <SessionProvider session={session}>
      <Layout>
        <Component {...pageProps} />
      </Layout>
    </SessionProvider>
  );
}
