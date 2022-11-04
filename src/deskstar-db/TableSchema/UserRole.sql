create table "UserRole"
(
    "UserID" uuid not null
        constraint "UserRole_User_null_fk"
            references "User",
    "RoleID" uuid not null
        constraint "UserRole_Role_null_fk"
            references "Role",
    constraint "UserRole_pk"
        primary key ("RoleID", "UserID")
);

alter table "UserRole"
    owner to postgres;

