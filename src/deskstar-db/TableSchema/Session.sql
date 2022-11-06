create table "Session" (
    "Token" uuid default gen_random_uuid() not null constraint "Session_pk" primary key,
    "UserID" uuid not null constraint "UserID_fk" references "User",
    "Timestamp" timestamp default CURRENT_TIMESTAMP(2) not null
);
alter table "Session" owner to postgres;