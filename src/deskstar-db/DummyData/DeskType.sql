create table "DeskType"
(
    "DeskTypeID"   uuid default gen_random_uuid() not null
        constraint "DeskType_pk"
            primary key,
    "DeskTypeName" varchar                        not null,
    "CompanyID"    uuid                           not null
        constraint "DeskType_Company_null_fk"
            references "Company"
);

alter table "DeskType"
    owner to postgres;

INSERT INTO public."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID") VALUES ('62c246f7-6b08-4d73-bf33-a8b74dc911f1', 'Standard Schreibtisch', 'c39e9222-f53c-49ef-80b4-88b85ce8cf71');
INSERT INTO public."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID") VALUES ('830f3735-90fd-4a39-bfa1-b1d8cbb1da67', 'Standard Schreibtisch', 'ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40');
INSERT INTO public."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID") VALUES ('80cbbf8a-de3f-4ffa-92e2-6269bc173c2d', 'Mahagoni Schreibtisch', 'ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40');
