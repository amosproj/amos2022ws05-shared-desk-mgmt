import Head from "next/head";
import Collapse from "../components/Collapse";
import { IDesk } from "../types/desk";
import DesksTable from "../components/DesksTable";
//TODO: delete this - just used for mockup data
import { GetServerSideProps } from "next";
import { desks } from "../desks";

const SearchResults = ({ results }: { results: IDesk[] }) => {
  // format results
  let formattedResults: any = {};
  for (const result of results) {
    if (!formattedResults[result.location])
      formattedResults[result.location] = {};

    if (!formattedResults[result.location][result.building])
      formattedResults[result.location][result.building] = {};

    if (!formattedResults[result.location][result.building][result.room])
      formattedResults[result.location][result.building][result.room] = [];

    formattedResults[result.location][result.building][result.room].push(
      result
    );
  }

  return (
    <>
      <Head>
        <title>Search Results</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Search Results</h1>
      {Object.keys(formattedResults).map(
        (location: string, locationIndex: number) => (
          <Collapse key={location} index={locationIndex} title={location}>
            {Object.keys(formattedResults[location]).map(
              (building: string, buildingIndex: number) => (
                <Collapse key={building} index={buildingIndex} title={building}>
                  {Object.keys(formattedResults[location][building]).map(
                    (room: string, roomIndex: number) => (
                      <Collapse key={room} index={roomIndex} title={room}>
                        <DesksTable
                          desks={formattedResults[location][building][room]}
                        />
                      </Collapse>
                    )
                  )}
                </Collapse>
              )
            )}
          </Collapse>
        )
      )}
    </>
  );
};

//TODO: delete this - this is just for developing this component
export const getServerSideProps: GetServerSideProps = async () => {
  const sortedDesks = desks.sort((a: IDesk, b: IDesk) =>
    a.deskName.localeCompare(b.deskName)
  );
  return {
    props: {
      results: sortedDesks,
    },
  };
};

export default SearchResults;
