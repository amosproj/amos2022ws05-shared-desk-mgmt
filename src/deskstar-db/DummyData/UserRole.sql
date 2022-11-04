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

INSERT INTO public."UserRole" ("UserID", "RoleID") VALUES ('80f4f525-9b0a-4687-b1b4-40bbfedea97a', '1da6cc02-7784-4258-8b95-fb0af2da65b4');
INSERT INTO public."UserRole" ("UserID", "RoleID") VALUES ('d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c', '95befa32-4419-4272-8465-f6d6983177b2');
INSERT INTO public."UserRole" ("UserID", "RoleID") VALUES ('d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c', '1da6cc02-7784-4258-8b95-fb0af2da65b4');
INSERT INTO public."UserRole" ("UserID", "RoleID") VALUES ('c4bf6f61-b2b8-4595-8fec-fe2abfa833a3', '95befa32-4419-4272-8465-f6d6983177b2');
