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

INSERT INTO public."Role" ("RoleID", "RoleName", "CompanyID") VALUES ('95befa32-4419-4272-8465-f6d6983177b2', 'FAU Admin', 'c39e9222-f53c-49ef-80b4-88b85ce8cf71');
INSERT INTO public."Role" ("RoleID", "RoleName", "CompanyID") VALUES ('1da6cc02-7784-4258-8b95-fb0af2da65b4', 'TU Berlin Admin', 'ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40');
