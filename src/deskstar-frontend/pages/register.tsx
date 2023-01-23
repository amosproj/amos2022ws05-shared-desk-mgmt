import Head from "next/head";
import Link from "next/link";
import RegisterPanel from "../components/RegisterPanel";
import { getCompanies } from "../lib/api/CompanyService";

export default function Register({
  companies,
}: {
  companies: {
    id: string;
    name: string;
  }[];
}) {
  return (
    <div className="flex flex-col items-center justify-center h-[80vh] gap-2">
      <Head>
        <title>Register</title>
      </Head>

      <RegisterPanel companies={companies} />

      <Link
        className="underline text-blue-500 hover:text-blue-600"
        href="/login"
      >
        Login
      </Link>
    </div>
  );
}

export async function getServerSideProps() {
  try {
    const companies = await getCompanies();

    return {
      props: {
        companies,
      },
    };
  } catch (e) {
    console.log(e);

    return {
      redirect: {
        destination: "/500?msg=Error+while+fetching+companies",
        permanent: false,
      },
    };
  }
}
