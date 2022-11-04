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

