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

INSERT INTO public."Building" ("BuildingID", "CompanyID", "BuildingName", "Location") VALUES ('a0a9cf1f-a145-4204-9181-5e0af5adeb06', 'c39e9222-f53c-49ef-80b4-88b85ce8cf71', 'Blaues Hochhaus', 'Martensstrasse 3
91058 Erlangen');
INSERT INTO public."Building" ("BuildingID", "CompanyID", "BuildingName", "Location") VALUES ('c6a9787b-40f6-454e-a4bf-dd334df3db22', 'ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40', 'Hauptgebäude', 'Strasse des 17. Juni 135 
10623 Berlin');
