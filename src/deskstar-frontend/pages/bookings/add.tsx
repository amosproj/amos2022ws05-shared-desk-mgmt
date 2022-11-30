import Head from "next/head";

import Collapse from "../../components/Collapse";
import { IRoom } from "../../types/room";

//TODO: delete this - just used for mockup data
import { GetServerSideProps } from "next";
import { rooms } from "../../rooms";
import { deskTypes } from "../../deskTypes";
import { IDeskType } from "../../types/desktypes";
import { useSession } from "next-auth/react";

const Bookings = ({
  results,
  types,
}: {
  results: IRoom[];
  types: IDeskType[];
}) => {
  let { data: session } = useSession();

  let buildings: string[] = [];
  let locations: string[] = [];
  let rooms: string[] = [];
  let deskTypes: string[] = [];

  // Fields to give to the backend
  let chosenBuildings: string[] = [];
  let chosenLocations: string[] = [];
  let chosenRooms: string[] = [];
  let chosenTypes: string[] = [];

  let startDateTime: string;
  let endDateTime: string;

  for (const result of results) {
    buildings.push(result.building);
    locations.push(result.location);
    rooms.push(result.roomName);
  }

  for (const type of types) {
    deskTypes.push(type.typeName);
  }

  return (
    <div>
      <Head>
        <title>Add New Booking</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Add New Booking</h1>
      <h1 className="text-2xl font-bold text-center mt-10">
        Hello {session?.user?.name}, book your personal desk.
      </h1>
      <br />
      <div className="form-group">
        <label className="form-label" htmlFor="start-date">
          <b>Start: </b> &nbsp;
        </label>
        <input
          className="form-input"
          type="datetime-local"
          id="start-date-time"
          name="Start"
          defaultValue={new Date()
            .toISOString()
            .substring(0, "YYYY-MM-DDTHH:SS".length)}
          min={new Date().toISOString().substring(0, "YYYY-MM-DDTHH:SS".length)}
          onChange={(event) => (startDateTime = event.target.value)}
        />
      </div>

      <div className="form-group">
        <label className="form-label" htmlFor="end-date">
          <b>End: </b> &nbsp;
        </label>
        <input
          className="form-input"
          type="datetime-local"
          id="end-date-time"
          min={new Date().toISOString().substring(0, "YYYY-MM-DDTHH:SS".length)}
          defaultValue={getEndDate()}
          onChange={(event) => (endDateTime = event.target.value)}
        />
      </div>

      <div className="dropdown dropdown-hover">
        <label tabIndex={0} className="btn m-1">
          Locations
        </label>
        <div className="form-control">
          <ul
            tabIndex={0}
            className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
          >
            <li key="li_allLocations">
              <label className="label cursor-pointer">
                <span className="label-text">All Locations</span>
                <input
                  id="allLocations"
                  type="checkbox"
                  className="checkbox"
                  onClick={() => {
                    locations.forEach((location) => {
                      var ebox = document.getElementById(location);
                      if (ebox != null && ebox instanceof HTMLInputElement) {
                        var box: HTMLInputElement = ebox;
                        if (!box.checked) {
                          chosenLocations.push(location);
                        }
                        box.checked = true;
                      }
                    });
                  }}
                />
              </label>
            </li>
            <div className="divider"></div>
            {locations.map((location: string) => (
              <li key={"li_" + location}>
                <label className="label cursor-pointer">
                  <span className="label-text">{location}</span>
                  <input
                    id={location}
                    type="checkbox"
                    className="checkbox"
                    onClick={() => {
                      if (chosenLocations.includes(location)) {
                        const index = chosenLocations.indexOf(location, 0);
                        if (index > -1) {
                          chosenLocations.splice(index, 1);
                          var allBox = document.getElementById("allLocations");
                          if (
                            allBox != null &&
                            allBox instanceof HTMLInputElement &&
                            allBox.checked
                          ) {
                            allBox.checked = false;
                          }
                        }
                      } else {
                        chosenLocations.push(location);
                      }
                    }}
                  />
                </label>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <div className="dropdown dropdown-hover">
        <label tabIndex={0} className="btn m-1">
          Buildings
        </label>
        <div className="form-control">
          <ul
            tabIndex={0}
            className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
          >
            <li key="li_allBuildings">
              <label className="label cursor-pointer">
                <span className="label-text">All Buildings</span>
                <input
                  id="allBuildings"
                  type="checkbox"
                  className="checkbox"
                  onClick={() => {
                    buildings.forEach((building) => {
                      var ebox = document.getElementById(building);
                      if (ebox != null && ebox instanceof HTMLInputElement) {
                        var box: HTMLInputElement = ebox;
                        if (!box.checked) {
                          chosenBuildings.push(building);
                        }
                        box.checked = true;
                      }
                    });
                  }}
                />
              </label>
            </li>
            <div className="divider"></div>

            {buildings.map((building: string) => (
              <li key={building}>
                <label className="label cursor-pointer">
                  <span className="label-text">{building}</span>
                  <input
                    id={building}
                    type="checkbox"
                    className="checkbox"
                    onClick={() => {
                      if (chosenBuildings.includes(building)) {
                        const index = chosenBuildings.indexOf(building, 0);
                        if (index > -1) {
                          chosenBuildings.splice(index, 1);
                          var allBox = document.getElementById("allBuildings");
                          if (
                            allBox != null &&
                            allBox instanceof HTMLInputElement &&
                            allBox.checked
                          ) {
                            allBox.checked = false;
                          }
                        }
                      } else {
                        chosenBuildings.push(building);
                      }
                    }}
                  />
                </label>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <div className="dropdown dropdown-hover">
        <label tabIndex={0} className="btn m-1">
          Rooms
        </label>
        <div className="form-control">
          <ul
            tabIndex={0}
            className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
          >
            <li key="li_allRooms">
              <label className="label cursor-pointer">
                <span className="label-text">All Rooms</span>
                <input
                  id="allRooms"
                  type="checkbox"
                  className="checkbox"
                  onClick={() => {
                    rooms.forEach((room) => {
                      var ebox = document.getElementById(room);

                      if (ebox != null && ebox instanceof HTMLInputElement) {
                        var box: HTMLInputElement = ebox;
                        if (!box.checked) {
                          chosenRooms.push(room);
                        }
                        box.checked = true;
                      }
                    });
                  }}
                />
              </label>
            </li>
            <div className="divider"></div>

            {rooms.map((room: string) => (
              <li key={room}>
                <label className="label cursor-pointer">
                  <span className="label-text">{room}</span>
                  <input
                    id={room}
                    type="checkbox"
                    className="checkbox"
                    onClick={() => {
                      if (chosenRooms.includes(room)) {
                        const index = chosenRooms.indexOf(room, 0);
                        if (index > -1) {
                          chosenRooms.splice(index, 1);
                          var allBox = document.getElementById("allRooms");
                          if (
                            allBox != null &&
                            allBox instanceof HTMLInputElement &&
                            allBox.checked
                          ) {
                            allBox.checked = false;
                          }
                        }
                      } else {
                        chosenRooms.push(room);
                      }
                    }}
                  />
                </label>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <div className="dropdown dropdown-hover">
        <label tabIndex={0} className="btn m-1">
          Types
        </label>
        <div className="form-control">
          <ul
            tabIndex={0}
            className="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52"
          >
            <li key="li_allTypes">
              <label className="label cursor-pointer">
                <span className="label-text">All Rooms</span>
                <input
                  id="allTypes"
                  type="checkbox"
                  className="checkbox"
                  onClick={() => {
                    deskTypes.forEach((type) => {
                      var ebox = document.getElementById(type);

                      if (ebox != null && ebox instanceof HTMLInputElement) {
                        var box: HTMLInputElement = ebox;
                        if (!box.checked) {
                          chosenTypes.push(type);
                        }
                        box.checked = true;
                      }
                    });
                  }}
                />
              </label>
            </li>
            <div className="divider"></div>

            {deskTypes.map((type: string) => (
              <li key={type}>
                <label className="label cursor-pointer">
                  <span className="label-text">{type}</span>
                  <input
                    id={type}
                    type="checkbox"
                    className="checkbox"
                    onClick={() => {
                      if (chosenTypes.includes(type)) {
                        const index = chosenTypes.indexOf(type, 0);
                        if (index > -1) {
                          chosenTypes.splice(index, 1);
                          var allBox = document.getElementById("allTypes");
                          if (
                            allBox != null &&
                            allBox instanceof HTMLInputElement &&
                            allBox.checked
                          ) {
                            allBox.checked = false;
                          }
                        }
                      } else {
                        chosenTypes.push(type);
                      }
                    }}
                  />
                </label>
              </li>
            ))}
          </ul>
        </div>
      </div>
      <br />
      <button type="button" className="btn btn-secondary" onClick={onClick}>
        Search for Desks
      </button>
    </div>
  );
};

//TODO: delete this - this is just for developing this component
export const getServerSideProps: GetServerSideProps = async () => {
  return {
    props: {
      results: rooms,
      types: deskTypes,
    },
  };
};

function onClick() {}

function getEndDate() {
  let date = new Date();
  date.setHours(date.getHours() + 1);
  return date.toISOString().substring(0, "YYYY-MM-DDTHH:SS".length);
}

export default Bookings;
