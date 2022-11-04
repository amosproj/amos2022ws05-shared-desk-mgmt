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

