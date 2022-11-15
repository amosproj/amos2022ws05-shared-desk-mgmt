--
-- PostgreSQL database cluster dump
--

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Drop databases (except postgres and template1)
--

DROP DATABASE deskstar;




--
-- Drop roles
--

DROP ROLE postgres;


--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:C/Dnq3xLlhtUILkv2VXSzA==$YK4DmBL0DR6pktZ6t3dWxagJb9invcprEyrdc2Ahc3I=:oKLHUPbOyyYSzVZBukGakQkuS3qqZYtN3AWcr0X0jl0=';

--
-- User Configurations
--

--
-- Databases
--

--
-- Database "deskstar" dump
--

--
-- PostgreSQL database dump
--

-- Dumped from database version 15.0 (Debian 15.0-1.pgdg110+1)
-- Dumped by pg_dump version 15.0 (Debian 15.0-1.pgdg110+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: deskstar; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE deskstar WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE deskstar OWNER TO postgres;

\connect deskstar

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Booking; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Booking" (
    "BookingID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "UserID" uuid NOT NULL,
    "DeskID" uuid NOT NULL,
    "Timestamp" timestamp without time zone DEFAULT CURRENT_TIMESTAMP(2) NOT NULL,
    "StartTime" timestamp without time zone NOT NULL,
    "EndTime" timestamp without time zone NOT NULL
);


ALTER TABLE public."Booking" OWNER TO postgres;

--
-- Name: Building; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Building" (
    "BuildingID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "BuildingName" character varying NOT NULL,
    "CompanyID" uuid NOT NULL,
    "Location" character varying NOT NULL
);


ALTER TABLE public."Building" OWNER TO postgres;

--
-- Name: Company; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Company" (
    "CompanyID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "CompanyName" character varying NOT NULL,
    "Logo" boolean
);


ALTER TABLE public."Company" OWNER TO postgres;

--
-- Name: Desk; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Desk" (
    "DeskID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "DeskName" character varying NOT NULL,
    "RoomID" uuid NOT NULL,
    "DeskTypeID" uuid NOT NULL
);


ALTER TABLE public."Desk" OWNER TO postgres;

--
-- Name: DeskType; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DeskType" (
    "DeskTypeID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "DeskTypeName" character varying NOT NULL,
    "CompanyID" uuid NOT NULL
);


ALTER TABLE public."DeskType" OWNER TO postgres;

--
-- Name: Floor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Floor" (
    "FloorID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "BuildingID" uuid NOT NULL,
    "FloorName" character varying NOT NULL
);


ALTER TABLE public."Floor" OWNER TO postgres;

--
-- Name: Role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Role" (
    "RoleID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "RoleName" character varying NOT NULL,
    "CompanyID" uuid NOT NULL
);


ALTER TABLE public."Role" OWNER TO postgres;

--
-- Name: Room; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Room" (
    "RoomID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "FloorID" uuid NOT NULL,
    "RoomName" character varying NOT NULL
);


ALTER TABLE public."Room" OWNER TO postgres;

--
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User" (
    "UserID" uuid DEFAULT gen_random_uuid() NOT NULL,
    "FirstName" character varying NOT NULL,
    "LastName" character varying NOT NULL,
    "MailAddress" character varying NOT NULL,
    "Password" character varying NOT NULL,
    "CompanyID" uuid NOT NULL,
    "IsApproved" boolean DEFAULT false
);


ALTER TABLE public."User" OWNER TO postgres;

--
-- Name: UserRole; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserRole" (
    "UserID" uuid NOT NULL,
    "RoleID" uuid NOT NULL
);


ALTER TABLE public."UserRole" OWNER TO postgres;

--
-- Data for Name: Booking; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") FROM stdin;
be4af0c9-3a0a-4f9f-89e6-9961249b875c	d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c	b72fe17d-915e-4c88-aa7d-bf1133d5808c	2022-11-03 20:13:40.94	2022-11-04 08:00:01	2022-11-04 11:00:01
f682fc33-6d6d-436b-a3ed-7b1bbccbf163	d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c	43eadd55-b972-4c7b-b11d-ba651900b732	2022-11-03 20:14:27.28	2022-11-04 14:00:00	2022-11-04 17:30:00
50fdf8e6-3c5f-4e8b-b227-e700080827ab	80f4f525-9b0a-4687-b1b4-40bbfedea97a	43eadd55-b972-4c7b-b11d-ba651900b732	2022-11-03 20:14:27.28	2022-11-04 07:00:00	2022-11-04 09:00:00
\.


--
-- Data for Name: Building; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Building" ("BuildingID", "CompanyID", "BuildingName", "Location") FROM stdin;
a0a9cf1f-a145-4204-9181-5e0af5adeb06	c39e9222-f53c-49ef-80b4-88b85ce8cf71	Blaues Hochhaus	Martensstrasse 3\n91058 Erlangen
c6a9787b-40f6-454e-a4bf-dd334df3db22	ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40	Hauptgebäude	Strasse des 17. Juni 135 \n10623 Berlin
\.


--
-- Data for Name: Company; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Company" ("CompanyName", "Logo", "CompanyID") FROM stdin;
FAU	\N	c39e9222-f53c-49ef-80b4-88b85ce8cf71
Interflex	\N	08dd2433-fd02-49a4-a9f8-6e98a1b9183c
TU Berlin	\N	ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40
\.


--
-- Data for Name: Desk; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID") FROM stdin;
b72fe17d-915e-4c88-aa7d-bf1133d5808c	Dirk Riehles Schreibtisch	01e5f9a4-b0c4-45ff-9ee6-3ffe259e42fb	62c246f7-6b08-4d73-bf33-a8b74dc911f1
43eadd55-b972-4c7b-b11d-ba651900b732	Schreibtisch der Präsidentin	7d2c8ebd-ec74-4c74-ab96-7d5a11b174aa	80cbbf8a-de3f-4ffa-92e2-6269bc173c2d
\.


--
-- Data for Name: DeskType; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID") FROM stdin;
62c246f7-6b08-4d73-bf33-a8b74dc911f1	Standard Schreibtisch	c39e9222-f53c-49ef-80b4-88b85ce8cf71
830f3735-90fd-4a39-bfa1-b1d8cbb1da67	Standard Schreibtisch	ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40
80cbbf8a-de3f-4ffa-92e2-6269bc173c2d	Mahagoni Schreibtisch	ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40
\.


--
-- Data for Name: Floor; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Floor" ("FloorID", "BuildingID", "FloorName") FROM stdin;
58b042ee-5a35-4f40-b995-9d8ca2ec5f0b	a0a9cf1f-a145-4204-9181-5e0af5adeb06	11. Stock
5c262286-45ea-4ac2-b6d0-354b57274bff	c6a9787b-40f6-454e-a4bf-dd334df3db22	1. Stock
\.


--
-- Data for Name: Role; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Role" ("RoleID", "RoleName", "CompanyID") FROM stdin;
95befa32-4419-4272-8465-f6d6983177b2	FAU Admin	c39e9222-f53c-49ef-80b4-88b85ce8cf71
1da6cc02-7784-4258-8b95-fb0af2da65b4	TU Berlin Admin	ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40
\.


--
-- Data for Name: Room; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Room" ("RoomID", "FloorID", "RoomName") FROM stdin;
01e5f9a4-b0c4-45ff-9ee6-3ffe259e42fb	58b042ee-5a35-4f40-b995-9d8ca2ec5f0b	11.132
7d2c8ebd-ec74-4c74-ab96-7d5a11b174aa	5c262286-45ea-4ac2-b6d0-354b57274bff	Büro der Präsidentin
\.


--
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."User" ("FirstName", "UserID", "CompanyID", "LastName", "MailAddress", "IsApproved", "Password") FROM stdin;
Jan	80f4f525-9b0a-4687-b1b4-40bbfedea97a	ba22608d-b2a0-4ef9-a27a-96ccbbb6cb40	Tiegges	jan.tiegges@campus.tu-berlin.de	t	AQAAAAEAACcQAAAAEE1xsMFLxrm2in8Wn4xBUlKW+UoG4BDH/SlBlzfdInZAfTPgck82hx6hZc0sku96cw==
Felix	c4bf6f61-b2b8-4595-8fec-fe2abfa833a3	c39e9222-f53c-49ef-80b4-88b85ce8cf71	Lang	felix.l.lang@fau.de	t	AQAAAAEAACcQAAAAEP+heqBW2asOFj+wdlYv9Px+jF0go/cvbLOlLMHAODwoURdbDGazj7bjLS/T1TBJUQ==
Fiona	d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c	c39e9222-f53c-49ef-80b4-88b85ce8cf71	Sternberg	fiona.sternberg@fau.de	t	AQAAAAEAACcQAAAAENF+iMBe/9g6jDq1pVE3FWwNhGmS8PZjhSSzdg7uWM7btuywOxryyeQFhqyDb0jQNg==
Dirk	3ff4962a-dcc2-4fe3-a2dd-3a777dc57421	c39e9222-f53c-49ef-80b4-88b85ce8cf71	Riehle	dirk.riehle@fau.de	f	AQAAAAEAACcQAAAAEMoqbzeAEutwHtOwwEi5i9ep5oGrS3V7jQYNbNSMs3KVgIqZWwlYjRyf8AoA40t0Lg==
Jochem	bc58af7c-85ed-4852-973f-865e9b5cdb9d	c39e9222-f53c-49ef-80b4-88b85ce8cf71	Kries	jochem.kries@allegion.com	t	AQAAAAEAACcQAAAAEEq95kTadJzUEJ/YzprHjUZyTEsQBlEhhp8IgZeEmXfkAFFxonMsod6lICyWT1ujdg==
\.


--
-- Data for Name: UserRole; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserRole" ("UserID", "RoleID") FROM stdin;
80f4f525-9b0a-4687-b1b4-40bbfedea97a	1da6cc02-7784-4258-8b95-fb0af2da65b4
d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c	95befa32-4419-4272-8465-f6d6983177b2
d9c68c89-c5d4-41cf-9ed7-f72cf9a7922c	1da6cc02-7784-4258-8b95-fb0af2da65b4
c4bf6f61-b2b8-4595-8fec-fe2abfa833a3	95befa32-4419-4272-8465-f6d6983177b2
\.


--
-- Name: Booking Booking_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Booking"
    ADD CONSTRAINT "Booking_pk" PRIMARY KEY ("BookingID");


--
-- Name: Building Building_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Building"
    ADD CONSTRAINT "Building_pk" PRIMARY KEY ("BuildingID");


--
-- Name: Company Company_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Company"
    ADD CONSTRAINT "Company_pk" PRIMARY KEY ("CompanyID");


--
-- Name: DeskType DeskType_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DeskType"
    ADD CONSTRAINT "DeskType_pk" PRIMARY KEY ("DeskTypeID");


--
-- Name: Desk Desk_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Desk"
    ADD CONSTRAINT "Desk_pk" PRIMARY KEY ("DeskID");


--
-- Name: Floor Floor_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Floor"
    ADD CONSTRAINT "Floor_pk" PRIMARY KEY ("FloorID");


--
-- Name: Role Role_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pk" PRIMARY KEY ("RoleID");


--
-- Name: Room Room_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Room"
    ADD CONSTRAINT "Room_pk" PRIMARY KEY ("RoomID");


--
-- Name: UserRole UserRole_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "UserRole_pk" PRIMARY KEY ("RoleID", "UserID");


--
-- Name: User User_Mail; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_Mail" UNIQUE ("MailAddress");


--
-- Name: User User_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pk" PRIMARY KEY ("UserID");


--
-- Name: Booking Booking_Desk_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Booking"
    ADD CONSTRAINT "Booking_Desk_null_fk" FOREIGN KEY ("DeskID") REFERENCES public."Desk"("DeskID");


--
-- Name: Booking Booking_User_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Booking"
    ADD CONSTRAINT "Booking_User_null_fk" FOREIGN KEY ("UserID") REFERENCES public."User"("UserID");


--
-- Name: User CompanyID_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "CompanyID_fk" FOREIGN KEY ("CompanyID") REFERENCES public."Company"("CompanyID");


--
-- Name: DeskType DeskType_Company_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DeskType"
    ADD CONSTRAINT "DeskType_Company_null_fk" FOREIGN KEY ("CompanyID") REFERENCES public."Company"("CompanyID");


--
-- Name: Desk Desk_DeskType_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Desk"
    ADD CONSTRAINT "Desk_DeskType_null_fk" FOREIGN KEY ("DeskTypeID") REFERENCES public."DeskType"("DeskTypeID");


--
-- Name: Desk Desk_Room_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Desk"
    ADD CONSTRAINT "Desk_Room_null_fk" FOREIGN KEY ("RoomID") REFERENCES public."Room"("RoomID");


--
-- Name: Floor Floor_Building_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Floor"
    ADD CONSTRAINT "Floor_Building_null_fk" FOREIGN KEY ("BuildingID") REFERENCES public."Building"("BuildingID");


--
-- Name: Role Role_Company_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_Company_null_fk" FOREIGN KEY ("CompanyID") REFERENCES public."Company"("CompanyID");


--
-- Name: Room Room_Floor_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Room"
    ADD CONSTRAINT "Room_Floor_null_fk" FOREIGN KEY ("FloorID") REFERENCES public."Floor"("FloorID");


--
-- Name: UserRole UserRole_Role_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "UserRole_Role_null_fk" FOREIGN KEY ("RoleID") REFERENCES public."Role"("RoleID");


--
-- Name: UserRole UserRole_User_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserRole"
    ADD CONSTRAINT "UserRole_User_null_fk" FOREIGN KEY ("UserID") REFERENCES public."User"("UserID");


--
-- Name: Building foreign_key_name; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Building"
    ADD CONSTRAINT foreign_key_name FOREIGN KEY ("CompanyID") REFERENCES public."Company"("CompanyID");


--
-- PostgreSQL database dump complete
--

--
-- PostgreSQL database cluster dump complete
--

