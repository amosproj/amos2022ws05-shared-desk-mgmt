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

