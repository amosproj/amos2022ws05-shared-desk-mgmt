create table "Role"
(
    "RoleID"    uuid default gen_random_uuid() not null
        constraint "Role_pk"
            primary key,
    "RoleName"  varchar                        not null,
    "CompanyID" uuid                           not null
        constraint "Role_Company_null_fk"
            references "Company"
);

alter table "Role"
    owner to postgres;

