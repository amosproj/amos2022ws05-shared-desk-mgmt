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

INSERT INTO public."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID") VALUES ('b72fe17d-915e-4c88-aa7d-bf1133d5808c', 'Dirk Riehles Schreibtisch', '01e5f9a4-b0c4-45ff-9ee6-3ffe259e42fb', '62c246f7-6b08-4d73-bf33-a8b74dc911f1');
INSERT INTO public."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID") VALUES ('43eadd55-b972-4c7b-b11d-ba651900b732', 'Schreibtisch der Präsidentin', '7d2c8ebd-ec74-4c74-ab96-7d5a11b174aa', '80cbbf8a-de3f-4ffa-92e2-6269bc173c2d');
