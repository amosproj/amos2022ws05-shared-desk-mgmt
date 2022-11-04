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

