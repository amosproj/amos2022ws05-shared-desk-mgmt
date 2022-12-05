import { IDesk } from "../types/desk";
import Collapse from "./Collapse";
import DesksTable from "./DesksTable";

export default function DeskSearchResults({ results }: { results: IDesk[] }) {
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
}
