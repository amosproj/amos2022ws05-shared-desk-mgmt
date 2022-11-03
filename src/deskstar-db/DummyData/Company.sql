create table "Company"
(
    "CompanyName" varchar                        not null,
    "Logo"        boolean,
    "CompanyID"   uuid default gen_random_uuid() not null
        constraint "Company_pk"
            primary key
);

alter table "Company"
    owner to postgres;

INSERT INTO public."Company" ("CompanyName", "Logo", "CompanyID") VALUES ('FAU', null, 'c39e9222-f53c-49ef-80b4-88b85ce8cf71');
INSERT INTO public."Company" ("CompanyName", "Logo", "CompanyID") VALUES ('Interflex', null, '08dd2433-fd02-49a4-a9f8-6e98a1b9183c');
INSERT INTO public."Company" ("CompanyName", "Logo", "CompanyID") VALUES ('TU Berlin', null, 'ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40');
