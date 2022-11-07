create table "Company" (
    "CompanyName" varchar not null,
    "Logo" boolean,
    "CompanyID" uuid default gen_random_uuid() not null constraint "Company_pk" primary key
);
alter table "Company" owner to postgres;
create table "DeskType" (
    "DeskTypeID" uuid default gen_random_uuid() not null constraint "DeskType_pk" primary key,
    "DeskTypeName" varchar not null,
    "CompanyID" uuid not null constraint "DeskType_Company_null_fk" references "Company"
);
alter table "DeskType" owner to postgres;
create table "Building" (
    "BuildingID" uuid default gen_random_uuid() not null constraint "Building_pk" primary key,
    "CompanyID" uuid not null constraint foreign_key_name references "Company",
    "BuildingName" varchar not null,
    "Location" varchar not null
);
alter table "Building" owner to postgres;
create table "Floor" (
    "FloorID" uuid default gen_random_uuid() not null constraint "Floor_pk" primary key,
    "BuildingID" uuid not null constraint "Floor_Building_null_fk" references "Building",
    "FloorName" varchar not null
);
alter table "Floor" owner to postgres;
create table "Room" (
    "RoomID" uuid default gen_random_uuid() not null constraint "Room_pk" primary key,
    "FloorID" uuid not null constraint "Room_Floor_null_fk" references "Floor",
    "RoomName" varchar not null
);
alter table "Room" owner to postgres;
create table "Desk" (
    "DeskID" uuid default gen_random_uuid() not null constraint "Desk_pk" primary key,
    "DeskName" varchar not null,
    "RoomID" uuid not null constraint "Desk_Room_null_fk" references "Room",
    "DeskTypeID" uuid not null constraint "Desk_DeskType_null_fk" references "DeskType"
);
alter table "Desk" owner to postgres;
create table "Role" (
    "RoleID" uuid default gen_random_uuid() not null constraint "Role_pk" primary key,
    "RoleName" varchar not null,
    "CompanyID" uuid not null constraint "Role_Company_null_fk" references "Company"
);
alter table "Role" owner to postgres;
create table "User" (
    "FirstName" varchar not null,
    "UserID" uuid default gen_random_uuid() not null constraint "User_pk" primary key,
    "CompanyID" uuid not null constraint "CompanyID_fk" references "Company",
    "LastName" varchar not null,
    "MailAddress" varchar not null constraint "User_Mail" unique,
    "Password" varchar not null 
);
alter table "User" owner to postgres;
create table "UserRole" (
    "UserID" uuid not null constraint "UserRole_User_null_fk" references "User",
    "RoleID" uuid not null constraint "UserRole_Role_null_fk" references "Role",
    constraint "UserRole_pk" primary key ("RoleID", "UserID")
);
alter table "UserRole" owner to postgres;
create table "Booking" (
    "BookingID" uuid default gen_random_uuid() not null constraint "Booking_pk" primary key,
    "UserID" uuid not null constraint "Booking_User_null_fk" references "User",
    "DeskID" uuid not null constraint "Booking_Desk_null_fk" references "Desk",
    "Timestamp" timestamp default CURRENT_TIMESTAMP(2) not null,
    "StartTime" timestamp not null,
    "EndTime" timestamp not null
);
alter table "Booking" owner to postgres;

create table "Session" (
    "Token" uuid default gen_random_uuid() not null constraint "Session_pk" primary key,
    "UserID" uuid not null constraint "UserID_fk" references "User",
    "Timestamp" timestamp default CURRENT_TIMESTAMP(2) not null
);
alter table "Session" owner to postgres;