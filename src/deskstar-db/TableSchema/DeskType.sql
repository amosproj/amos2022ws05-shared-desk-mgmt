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

