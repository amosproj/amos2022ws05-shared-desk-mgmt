import Head from "next/head";
import Collapse from "../components/Collapse";
import { IDesk } from "../types/desk";
import DesksTable from "../components/DesksTable";

const SearchResults = ({ results }: { results: IDesk[] }) => {
  // format results
  let formattedResults: any = {};
  for (const result of results) {
    if (!formattedResults[result.location])
      formattedResults[result.location] = {};

    if (!formattedResults[result.location][result.buildingName])
      formattedResults[result.location][result.buildingName] = {};

    if (
      !formattedResults[result.location][result.buildingName][result.roomName]
    )
      formattedResults[result.location][result.buildingName][result.roomName] =
        [];

    formattedResults[result.location][result.buildingName][
      result.roomName
    ].push(result);
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

export default SearchResults;
