import dayjs, { Dayjs } from "dayjs";
import { useMemo, useState } from "react";
import { toast } from "react-toastify";
import { classes } from "../lib/helpers";
import { IBooking } from "../types/booking";

interface UpdateBookingModalProps {
  id: string;
  booking: IBooking;
  onUpdate: (booking: IBooking, startDateTime: Date, endDateTime: Date) => void;
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

  const minimumEndDateTime = useMemo(() => {
    return dayjs(startDateTime).add(1, "hour");
  }, [startDateTime]);

  async function updateBooking() {
    if (endDateTime.isBefore(minimumEndDateTime)) {
      toast.error(
        "The End Time needs to be at minimum 1 hour after the start time."
      );
      return;
    }
    if (
      !startDateTime ||
      !endDateTime ||
      !dayjs(startDateTime).isValid() ||
      !dayjs(endDateTime).isValid()
    )
      return toast.error("Please select a start and end time");

    await onUpdate(booking, startDateTime.toDate(), endDateTime.toDate());
  }

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
                className={classes(
                  "input input-bordered",
                  dayjs(endDateTime).isBefore(dayjs(minimumEndDateTime))
                    ? "border-red-600"
                    : ""
                )}
                type="datetime-local"
                // Bind the value of the input to enddatetime
                value={formatDateForInputField(endDateTime)}
                min={formatDateForInputField(today)}
                onChange={(event) => {
                  return setEndDateTime(dayjs(event.target.value));
                }}
              />
            </div>
            {dayjs(endDateTime).isBefore(dayjs(minimumEndDateTime)) && (
              <span className="text-red-600">
                The End Time needs to be at minimum 1 hour
                <br /> after the start time.
              </span>
            )}
          </form>
          <div className="modal-action">
            <label htmlFor={id} className="btn" onClick={() => updateBooking()}>
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
