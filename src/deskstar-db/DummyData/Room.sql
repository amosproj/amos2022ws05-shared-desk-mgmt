create table "Room"
(
    "RoomID"   uuid default gen_random_uuid() not null
        constraint "Room_pk"
            primary key,
    "FloorID"  uuid                           not null
        constraint "Room_Floor_null_fk"
            references "Floor",
    "RoomName" varchar                        not null
);

alter table "Room"
    owner to postgres;

INSERT INTO public."Room" ("RoomID", "FloorID", "RoomName") VALUES ('01e5f9a4-b0c4-45ff-9ee6-3ffe259e42fb', '58b042ee-5a35-4f40-b995-9d8ca2ec5f0b', '11.132');
INSERT INTO public."Room" ("RoomID", "FloorID", "RoomName") VALUES ('7d2c8ebd-ec74-4c74-ab96-7d5a11b174aa', '5c262286-45ea-4ac2-b6d0-354b57274bff', 'Büro der Präsidentin');
