create table "Booking"
(
    "BookingID" uuid      default gen_random_uuid()    not null
        constraint "Booking_pk"
            primary key,
    "UserID"    uuid                                   not null
        constraint "Booking_User_null_fk"
            references "User",
    "DeskID"    uuid                                   not null
        constraint "Booking_Desk_null_fk"
            references "Desk",
    "Timestamp" timestamp default CURRENT_TIMESTAMP(2) not null,
    "StartTime" timestamp                              not null,
    "EndTime"   timestamp                              not null
);

alter table "Booking"
    owner to postgres;

