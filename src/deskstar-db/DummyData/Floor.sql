create table "Floor"
(
    "FloorID"    uuid default gen_random_uuid() not null
        constraint "Floor_pk"
            primary key,
    "BuildingID" uuid                           not null
        constraint "Floor_Building_null_fk"
            references "Building",
    "FloorName"  varchar                        not null
);

alter table "Floor"
    owner to postgres;

INSERT INTO public."Floor" ("FloorID", "BuildingID", "FloorName") VALUES ('58b042ee-5a35-4f40-b995-9d8ca2ec5f0b', 'a0a9cf1f-a145-4204-9181-5e0af5adeb06', '11. Stock');
INSERT INTO public."Floor" ("FloorID", "BuildingID", "FloorName") VALUES ('5c262286-45ea-4ac2-b6d0-354b57274bff', 'c6a9787b-40f6-454e-a4bf-dd334df3db22', '1. Stock');
