create table "Building"
(
    "BuildingID"   uuid default gen_random_uuid() not null
        constraint "Building_pk"
            primary key,
    "CompanyID"    uuid                           not null
        constraint foreign_key_name
            references "Company",
    "BuildingName" varchar                        not null,
    "Location"     varchar                        not null
);

alter table "Building"
    owner to postgres;

