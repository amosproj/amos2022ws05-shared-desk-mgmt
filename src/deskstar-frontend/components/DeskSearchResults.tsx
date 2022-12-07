import { IDesk } from "../types/desk";
import Collapse from "./Collapse";
import DesksTable from "./DesksTable";

export default function DeskSearchResults({
  results,
  onBook,
}: {
  results: IDesk[];
  onBook: Function;
}) {
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
    <div>
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
                          onBook={onBook}
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
    </div>
  );
}
