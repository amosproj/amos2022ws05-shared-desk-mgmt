create table "Desk"
(
    "DeskID"     uuid default gen_random_uuid() not null
        constraint "Desk_pk"
            primary key,
    "DeskName"   varchar                        not null,
    "RoomID"     uuid                           not null
        constraint "Desk_Room_null_fk"
            references "Room",
    "DeskTypeID" uuid                           not null
        constraint "Desk_DeskType_null_fk"
            references "DeskType"
);

alter table "Desk"
    owner to postgres;

