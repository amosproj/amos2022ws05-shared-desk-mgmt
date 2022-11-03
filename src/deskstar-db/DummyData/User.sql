create table "User"
(
    "FirstName"   varchar                        not null,
    "UserID"      uuid default gen_random_uuid() not null
        constraint "User_pk"
            primary key,
    "CompanyID"   uuid                           not null
        constraint "CompanyID_fk"
            references "Company",
    "LastName"    varchar                        not null,
    "MailAddress" varchar                        not null
        constraint "User_Mail"
            unique
);

alter table "User"
    owner to postgres;

INSERT INTO public."User" ("FirstName", "UserID", "CompanyID", "LastName", "MailAddress") VALUES ('Felix', 'c4bf6f61-b2b8-4595-8fec-fe2abfa833a3', 'c39e9222-f53c-49ef-80b4-88b85ce8cf71', 'Lang', 'felix.l.lang@fau.de');
INSERT INTO public."User" ("FirstName", "UserID", "CompanyID", "LastName", "MailAddress") VALUES ('Fiona', 'd9c68c89-c5d4-41cf-9ed7-f72cf9a7922c', 'c39e9222-f53c-49ef-80b4-88b85ce8cf71', 'Sternberg', 'fiona.sternberg@fau.de');
INSERT INTO public."User" ("FirstName", "UserID", "CompanyID", "LastName", "MailAddress") VALUES ('Jan', '80f4f525-9b0a-4687-b1b4-40bbfedea97a', 'ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40', 'Tiegges', 'jan.tiegges@campus.tu-berlin.de');
