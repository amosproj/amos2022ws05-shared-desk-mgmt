import dayjs, { Dayjs } from "dayjs";
import { useState } from "react";
import { start } from "repl";
import { IBooking } from "../types/booking";

interface UpdateBookingModalProps {
  id: string;
  booking: IBooking;
  onUpdate: Function;
}

export function UpdateBookingModal({
  id,
  booking,
  onUpdate,
}: UpdateBookingModalProps) {
  const [startDateTime, setStartDateTime] = useState(
    dayjs(booking.startTime, {
      utc: true,
    })
  );
  const [endDateTime, setEndDateTime] = useState(
    dayjs(booking.endTime, {
      utc: true,
    })
  );

  let today = dayjs();
  today = today
    .set("hour", 8)
    .set("minute", 0)
    .set("second", 0)
    .set("millisecond", 0);

  return (
    <>
      <input type="checkbox" id={id} className="modal-toggle" />
      <div className="modal">
        <div className="modal-box">
          <label htmlFor={id} className="btn btn-sm btn-circle float-right">
            x
          </label>
          <h1 className="text-2xl pb-4">Update Booking</h1>
          <form>
            <InputForm
              label={"Desk:"}
              value={booking.deskName}
              disabled={true}
            />
            <InputForm label={"Room:"} value={booking.room} disabled={true} />
            <InputForm
              label={"Building:"}
              value={booking.building}
              disabled={true}
            />
            <div className="form-control">
              <label className="label" htmlFor="start-date">
                <b>Start: </b>
              </label>
              <input
                className="input input-bordered"
                type="datetime-local"
                name="Start"
                value={formatDateForInputField(startDateTime)}
                min={formatDateForInputField(today)}
                onChange={(event) =>
                  setStartDateTime(dayjs(event.target.value))
                }
              />
            </div>

            <div className="form-control">
              <label className="label" htmlFor="end-date">
                <b>End: </b>
              </label>
              <input
                className="input input-bordered"
                type="datetime-local"
                // Bind the value of the input to enddatetime
                value={formatDateForInputField(endDateTime)}
                min={formatDateForInputField(dayjs(today))}
                onChange={(event) => {
                  setEndDateTime(dayjs(event.target.value));
                }}
              />
            </div>
          </form>
          <div className="modal-action">
            <label
              htmlFor={id}
              className="btn"
              onClick={() => onUpdate(booking, startDateTime, endDateTime)}
            >
              Update
            </label>
          </div>
        </div>
      </div>
    </>
  );
}

interface InputFormProps {
  label: string;
  type?: string;
  value?: string;
  disabled?: boolean;
}

function InputForm({ label, type, value, disabled }: InputFormProps) {
  return (
    <div className="form-control">
      <label className="label">
        <b>{label}</b>
      </label>
      <input
        className="input input-bordered"
        type={type ?? "text"}
        value={value ?? ""}
        disabled={disabled ?? false}
      />
    </div>
  );
}

function formatDateForInputField(date: Dayjs) {
  return date.format("YYYY-MM-DDTHH:mm");
}
