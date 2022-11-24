import Head from "next/head";

import Collapse from "../components/Collapse";
import { IRoom } from "../types/room";

//TODO: delete this - just used for mockup data
import { GetServerSideProps } from "next";
import { rooms } from "../rooms";



const Bookings = ({results} : {results: IRoom[]}) => {
    const username = 'Test User';

    // TODO: Edit this to fit usecase
    let formattedResults: any = {};
    for (const result of results) {
      if (!formattedResults[result.location])
        formattedResults[result.location] = {};

      if (!formattedResults[result.location][result.building])
        formattedResults[result.location][result.building] = {};

      if (!formattedResults[result.location][result.building][result])
        formattedResults[result.location][result.building][result] = [];

      formattedResults[result.location][result.building][result].push(
        result
      );
    }
    
  return (
    <div>
      <Head>
        <title>Add New Booking</title>
      </Head>
      <h1 className="text-3xl font-bold text-center my-10">Add New Booking</h1>
      <div>Hello {username}, book your personal desk.</div>
      <Collapse key="buildings" index={0} title="Buildings">
        {Object.keys(results).map(
          (building : string, buildingIndex: number) => (
            <div key={buildingIndex}>{results[building]}</div>
          )
        )}
      </Collapse>
      <button type="button" onClick={onClick}>Search for Desks</button>
    </div>
  );
}

//TODO: delete this - this is just for developing this component
export const getServerSideProps: GetServerSideProps = async () => {
  return {
    props: {
      results: rooms,
    },
  };
};

function onClick() {

}

export default Bookings;