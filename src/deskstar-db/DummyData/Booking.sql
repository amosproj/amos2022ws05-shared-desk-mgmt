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

INSERT INTO public."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('be4af0c9-3a0a-4f9f-89e6-9961249b875c', 'd9c68c89-c5d4-41cf-9ed7-f72cf9a7922c', 'b72fe17d-915e-4c88-aa7d-bf1133d5808c', '2022-11-03 20:13:40.940000', '2022-11-04 08:00:01.000000', '2022-11-04 11:00:01.000000');
INSERT INTO public."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('f682fc33-6d6d-436b-a3ed-7b1bbccbf163', 'd9c68c89-c5d4-41cf-9ed7-f72cf9a7922c', '43eadd55-b972-4c7b-b11d-ba651900b732', '2022-11-03 20:14:27.280000', '2022-11-04 14:00:00.000000', '2022-11-04 17:30:00.000000');
INSERT INTO public."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('50fdf8e6-3c5f-4e8b-b227-e700080827ab', '80f4f525-9b0a-4687-b1b4-40bbfedea97a', '43eadd55-b972-4c7b-b11d-ba651900b732', '2022-11-03 20:14:27.280000', '2022-11-04 07:00:00.000000', '2022-11-04 09:00:00.000000');
