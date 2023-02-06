--
-- PostgreSQL database dump
--


--
-- Data for Name: Company; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Company" ("CompanyID", "CompanyName", "Logo") VALUES ('8745f064-c658-4a32-83fb-9d7d7e6d8f17', 'ACME Ltd.', NULL);
INSERT INTO "public"."Company" ("CompanyID", "CompanyName", "Logo") VALUES ('02c03441-5caa-4a7e-a710-b64cf62cc7aa', 'Sola Ltd.', NULL);
INSERT INTO "public"."Company" ("CompanyID", "CompanyName", "Logo") VALUES ('5e2761c1-86ae-4cbe-8b40-c1251ee38777', 'FAU (Friedrich-Alexander-Universität Erlangen-Nürnberg)', NULL);
INSERT INTO "public"."Company" ("CompanyID", "CompanyName", "Logo") VALUES ('eb6aa331-ad47-4f25-8a4a-1a9a40399da5', 'TU Berlin (Technische Universität Berlin)', NULL);
INSERT INTO "public"."Company" ("CompanyID", "CompanyName", "Logo") VALUES ('6fb5df25-64b1-4986-b072-04a5f1285b80', 'FU Berlin (Freie Universität Berlin)', NULL);


--
-- Data for Name: Building; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('3de7afbf-0289-4ba6-bada-a34353c5548a', 'Innovation Center', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', 'Platz d. Luftbrücke 5, 12101 Berlin', false);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('5fcde910-ca65-4636-84dd-54bb250252cd', 'Headquarter', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', 'Panoramastraße 1A, 10178 Berlin', true);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('d4747f4c-519b-465a-9c1d-2363cbbdec75', 'Headquarter', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', 'Sunshine Avenue 154, 99999 Sunset Valley', false);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('89d70192-e7be-45a7-a5d6-3fd894f2d0b9', 'Factory', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', 'Moonlight Road 78, 99999 Sunset Valley', false);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('a4e44b37-c1a8-4d57-9969-abb53975f9a6', 'IT-Center', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', 'Sunshine Avenue 154, 99999 Sunset Valley', false);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('99b374ab-92c3-4e91-82b6-58c938c08327', 'Blue Skyscraper', '5e2761c1-86ae-4cbe-8b40-c1251ee38777', 'Martensstraße 3, 91058 Erlangen', false);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('9cdbfdda-0d37-4780-a6df-2d05ad9d45bc', 'Main Building', '6fb5df25-64b1-4986-b072-04a5f1285b80', 'Kaiserswerther Str. 16-18, 14195 Berlin', false);
INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location", "IsMarkedForDeletion") VALUES ('b3f2328b-8657-476e-9378-c3bbfb27c1c5', 'Main Building', 'eb6aa331-ad47-4f25-8a4a-1a9a40399da5', 'Straße des 17. Juni 135, 10623 Berlin', false);


--
-- Data for Name: DeskType; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('64179bd0-98ad-460a-9e8c-ddd9102a35c8', 'Height-Adjustable Desk', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', 'Standard Desk', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', 'Standard', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('251a0802-2d7d-43f4-8b43-d661131bfd53', 'Height-Adjustable', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('8159b487-8761-472d-bfa7-157032becb4a', 'Special Hardware', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('df7a2073-0757-40ab-8665-00372f15156e', 'Big Desk', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', 'Telephone', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', 'Standard', '5e2761c1-86ae-4cbe-8b40-c1251ee38777', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('a8ac0d24-ecdb-4a15-b867-5e26407c59f2', 'Standard', 'eb6aa331-ad47-4f25-8a4a-1a9a40399da5', false);
INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID", "IsMarkedForDeletion") VALUES ('2a666a5f-a5f7-4d55-9cab-118c602eacbc', 'Standard', '6fb5df25-64b1-4986-b072-04a5f1285b80', false);


--
-- Data for Name: Floor; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('38420525-6e54-43fb-a456-fd37ef853a1e', '5fcde910-ca65-4636-84dd-54bb250252cd', '2nd Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('604f7e75-f894-459e-ac05-f5845368243b', '3de7afbf-0289-4ba6-bada-a34353c5548a', 'Ground Floor_B1', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('7b5944f3-98ab-49e1-82cf-3238166f7b9d', '5fcde910-ca65-4636-84dd-54bb250252cd', 'Ground Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('ffec69ab-736a-424a-a650-0d4a2653c370', '3de7afbf-0289-4ba6-bada-a34353c5548a', '1st Floor_B1', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('7f19935f-3137-41e9-aa84-e6e4d31fc374', '5fcde910-ca65-4636-84dd-54bb250252cd', '1st Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('6fd249d3-ab2b-4b51-ae5f-05bc66af3057', 'd4747f4c-519b-465a-9c1d-2363cbbdec75', 'First Floor HQ', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('0560d0e9-9286-4d6f-8d0f-d0f720407475', 'd4747f4c-519b-465a-9c1d-2363cbbdec75', 'Second Floor HQ', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('b9e83a4c-288a-40ce-b1da-62b74aef3691', 'd4747f4c-519b-465a-9c1d-2363cbbdec75', 'Third Floor HQ', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('d86ab795-8df0-4d9a-b418-e8d6be42fd24', 'a4e44b37-c1a8-4d57-9969-abb53975f9a6', 'First Floor IT', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('0e866325-aae4-4390-a2cb-d019a093d4a3', 'a4e44b37-c1a8-4d57-9969-abb53975f9a6', 'Second Floor IT', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('b558ce6d-61e6-4403-b827-3587c14c2048', '89d70192-e7be-45a7-a5d6-3fd894f2d0b9', 'Ground Floor Factory', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('fcde79fb-ac5c-4ea2-827f-e5622dc69b7d', '99b374ab-92c3-4e91-82b6-58c938c08327', 'First Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('612b0887-0ebf-421c-81ac-499809fa2d1b', '99b374ab-92c3-4e91-82b6-58c938c08327', 'Second Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('c03c700a-a4c2-4104-9996-10df8d1d73c8', 'b3f2328b-8657-476e-9378-c3bbfb27c1c5', 'First Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('b0c8c4ca-3678-46f3-adfc-21fbf4594cd8', 'b3f2328b-8657-476e-9378-c3bbfb27c1c5', 'Second Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('0dcd237e-6e38-409c-910d-0262635cb4d2', '9cdbfdda-0d37-4780-a6df-2d05ad9d45bc', 'First Floor', false);
INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName", "IsMarkedForDeletion") VALUES ('64362148-6653-4ef7-b3b4-6a4c86047a7b', '9cdbfdda-0d37-4780-a6df-2d05ad9d45bc', 'Second Floor', false);


--
-- Data for Name: Room; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('01f876b4-1f10-4163-bb4e-5f6b9fbf32e3', '604f7e75-f894-459e-ac05-f5845368243b', 'Mind Space', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('2f68d359-550a-428f-82be-7eb895d77371', 'ffec69ab-736a-424a-a650-0d4a2653c370', 'Silent Space', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('66d3ec54-811b-4449-bcc8-bcc270788a51', '38420525-6e54-43fb-a456-fd37ef853a1e', 'Social Space', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('6c37ba8b-9eb1-49e1-a009-dda96788205c', '7f19935f-3137-41e9-aa84-e6e4d31fc374', '100.1', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('80accedd-f2a8-4782-9316-03fd82430486', '7f19935f-3137-41e9-aa84-e6e4d31fc374', '100.2', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('a70fcf62-c27e-4bbe-881d-77758a0ded27', 'ffec69ab-736a-424a-a650-0d4a2653c370', 'Call Space', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('b2746b05-bcfd-4f93-9cee-d2a4e7d61357', '7b5944f3-98ab-49e1-82cf-3238166f7b9d', 'Open Space', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', '0560d0e9-9286-4d6f-8d0f-d0f720407475', 'Rainbow', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('c7288c00-4085-42ea-bd77-ff155179c723', '0560d0e9-9286-4d6f-8d0f-d0f720407475', 'Starlight', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('4b1221d2-2283-4d03-ab59-a1bede3aea74', '0560d0e9-9286-4d6f-8d0f-d0f720407475', 'Sunshine', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('57133c5f-d883-4cfd-863c-cafa60b8e92e', 'ffec69ab-736a-424a-a650-0d4a2653c370', 'Test', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('989fc44d-38da-4580-bb09-5c1b56c2e3de', '6fd249d3-ab2b-4b51-ae5f-05bc66af3057', 'Conference Room', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('8e7e1a2c-b1af-4702-b410-8d9d36e622af', '0e866325-aae4-4390-a2cb-d019a093d4a3', 'Coworking Space', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('985e09aa-c902-49ca-8e63-d5f224852595', '0560d0e9-9286-4d6f-8d0f-d0f720407475', 'Cloud', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('1d345f5b-1e27-4e41-92e9-62063929cecb', 'b9e83a4c-288a-40ce-b1da-62b74aef3691', 'Call Center', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('ee86f122-0614-40b6-8a74-888c853bf370', 'b558ce6d-61e6-4403-b827-3587c14c2048', 'Main Office', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('5b3332b9-911e-4f9b-97e8-86b12b0621cb', 'd86ab795-8df0-4d9a-b418-e8d6be42fd24', 'Big Office', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('729fb62a-8220-40e6-8a91-7b12ea18b6dd', 'd86ab795-8df0-4d9a-b418-e8d6be42fd24', 'Small Office', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('9a31d011-d739-4277-bb3e-7f910650b3d7', 'fcde79fb-ac5c-4ea2-827f-e5622dc69b7d', 'Office 1', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('e3389959-c161-4e8f-a3d5-d7919adbcbaf', 'fcde79fb-ac5c-4ea2-827f-e5622dc69b7d', 'Office 2', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('7c5351ca-150e-40e2-9369-e6c245c37d2c', '612b0887-0ebf-421c-81ac-499809fa2d1b', 'Office 3', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('6e9a2400-4f87-41d9-ad34-5de909986130', '612b0887-0ebf-421c-81ac-499809fa2d1b', 'Office 4', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('928f46b8-d74d-4a6a-b517-e0846806fe1f', 'c03c700a-a4c2-4104-9996-10df8d1d73c8', 'Office 1', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('84b76c97-d7f9-4529-805b-494195e37448', 'c03c700a-a4c2-4104-9996-10df8d1d73c8', 'Office 2', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('2d1e3874-f815-4b26-8ba1-ddf4e86abb48', 'b0c8c4ca-3678-46f3-adfc-21fbf4594cd8', 'Office 3', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('54176838-810c-4332-8e04-4ed411ed5091', 'b0c8c4ca-3678-46f3-adfc-21fbf4594cd8', 'Office 4', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('29918eb4-f4ae-44fb-b1a1-76050a04c403', '0dcd237e-6e38-409c-910d-0262635cb4d2', 'Office 1', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('0129358e-515a-4d31-a531-59da6134ff66', '0dcd237e-6e38-409c-910d-0262635cb4d2', 'Office 2', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('bbde0c96-5ca2-44bd-931e-6b6624187781', '64362148-6653-4ef7-b3b4-6a4c86047a7b', 'Office 3', false);
INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName", "IsMarkedForDeletion") VALUES ('17194565-4192-4c6e-ae98-dc4bdb54ea05', '64362148-6653-4ef7-b3b4-6a4c86047a7b', 'Office 4', false);


--
-- Data for Name: Desk; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('223386c1-8c71-4f6e-92b0-d5a54e2d268b', '1', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('330f037c-2381-4ec5-abca-4cb7ddbe7332', 'C', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('40c4e435-ed31-4d39-ab6a-68a21131d256', '2', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('527c3b1a-8fdf-4e07-99ec-95b7ed7041db', 'E', '66d3ec54-811b-4449-bcc8-bcc270788a51', '64179bd0-98ad-460a-9e8c-ddd9102a35c8', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('52b74553-a686-4b6e-8bda-07e17f5a78b5', 'B', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('672c5918-6e98-4cef-8e08-94f0d58d7861', '6', '6c37ba8b-9eb1-49e1-a009-dda96788205c', '64179bd0-98ad-460a-9e8c-ddd9102a35c8', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', 'A', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('81979b01-37a0-4377-a13b-785f81304271', 'F', '66d3ec54-811b-4449-bcc8-bcc270788a51', '64179bd0-98ad-460a-9e8c-ddd9102a35c8', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('82628286-ea9a-4587-9628-a88da14f21c2', '4', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('8b3320e1-a1cb-48a0-84ca-ce58f813b584', 'D', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('ac5d015a-4c2c-401e-9d4a-b413cce53bce', '3', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('f81e33fc-434f-4f61-990e-7532af2a325b', '5', '6c37ba8b-9eb1-49e1-a009-dda96788205c', '64179bd0-98ad-460a-9e8c-ddd9102a35c8', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('1af9a523-e3e3-4837-a425-444a35322e25', 'Window Desk Right', '4b1221d2-2283-4d03-ab59-a1bede3aea74', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('752d5131-0a70-40d8-aa1d-206a4ca1b11b', 'Window Desk Left', '4b1221d2-2283-4d03-ab59-a1bede3aea74', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('602e9087-da0a-488f-ac0e-5435091baf04', 'Middle Desk Right', '4b1221d2-2283-4d03-ab59-a1bede3aea74', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('d98d4b79-a387-4ecc-97df-e3bfda443dce', 'Middle Desk Left', '4b1221d2-2283-4d03-ab59-a1bede3aea74', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('5ce6ebbe-6401-4e4e-b006-876dcf50ce6b', 'Door Desk Right', '4b1221d2-2283-4d03-ab59-a1bede3aea74', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('4d49e2b5-fece-45e0-a87f-23340e68e447', 'Door Desk Left', '4b1221d2-2283-4d03-ab59-a1bede3aea74', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('84ceee51-b4db-4861-8060-9f15ca2eab75', 'Desk A', '5b3332b9-911e-4f9b-97e8-86b12b0621cb', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('1a742432-c63d-47f8-ad48-26ee7508c1a3', 'Desk B', '5b3332b9-911e-4f9b-97e8-86b12b0621cb', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0edbdaa7-5f03-4089-9bde-2f8d474486d2', 'Desk C', '5b3332b9-911e-4f9b-97e8-86b12b0621cb', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('dcd67a2a-bc3c-4860-9713-c08be2b939a0', 'Desk D', '5b3332b9-911e-4f9b-97e8-86b12b0621cb', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('709832a9-8115-4353-8d3c-69743176c423', 'Hardware Desk 1', '5b3332b9-911e-4f9b-97e8-86b12b0621cb', '8159b487-8761-472d-bfa7-157032becb4a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('49298187-a23d-47bd-90f9-1999133d1530', 'Hardware Desk 2', '5b3332b9-911e-4f9b-97e8-86b12b0621cb', '8159b487-8761-472d-bfa7-157032becb4a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e58a4136-d126-4114-9014-b82d6798ab6a', 'Conference Table', '989fc44d-38da-4580-bb09-5c1b56c2e3de', 'df7a2073-0757-40ab-8665-00372f15156e', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0e1c06e2-c3ee-44dc-8fab-4e7fabe62f34', 'Window Desk Right', 'bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('2109cde5-1ec7-46e3-9c0d-40dbc90c485b', 'Window Desk Left', 'bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('78d00410-fa4b-4fdb-9a53-d69c80c36a89', 'Middle Desk Right', 'bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('ea5bd3fd-b8ad-4ca4-9efc-ac5b673f7064', 'Middle Desk Left', 'bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e70f7bf2-56a8-4a38-877a-2f8f43ddb534', 'Door Desk Right', 'bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('36ebabfa-2448-46f5-9f5f-e56d8db6104f', 'Door Desk Left', 'bcae4ecd-b02b-4a5d-b0e1-29317ed5f571', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('721991b2-5e22-4cdd-81a5-38b31214dbe0', 'Northern Desk', 'c7288c00-4085-42ea-bd77-ff155179c723', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('bf280f41-7317-4db7-8ab4-68a2410de3c3', 'Southern Desk', 'c7288c00-4085-42ea-bd77-ff155179c723', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('b1d25d96-d5e9-4300-82a9-96fd88ae0e3f', 'Western Desk', 'c7288c00-4085-42ea-bd77-ff155179c723', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('d90c6dec-53cd-4070-9baf-5d4bf01215a2', 'Eastern Desk', 'c7288c00-4085-42ea-bd77-ff155179c723', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('958cc054-6d91-4e1d-9756-1515c7bf0291', 'Desk 1', '985e09aa-c902-49ca-8e63-d5f224852595', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('6219d7c3-c5c9-4b0d-9425-bd63ce8741b6', 'Desk 2', '985e09aa-c902-49ca-8e63-d5f224852595', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('f5aa5aa6-31fb-42fa-8de2-fe3b83ce83e6', 'Desk 3', '985e09aa-c902-49ca-8e63-d5f224852595', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('db4aaacb-adbb-4bd2-bb21-c942e308308c', 'Desk 4', '985e09aa-c902-49ca-8e63-d5f224852595', 'c4cfa7df-f23b-47b1-bdc5-1e1b43bb1756', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('c8b09995-41b5-4222-800f-8b149508bde0', 'Caller Desk 1', '1d345f5b-1e27-4e41-92e9-62063929cecb', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('8a161215-3c68-4c3f-9b57-e2661871a5df', 'Caller Desk 2', '1d345f5b-1e27-4e41-92e9-62063929cecb', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('23b496c9-1bc9-4380-bf8b-962897b00610', 'Caller Desk 3', '1d345f5b-1e27-4e41-92e9-62063929cecb', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0c55f16d-9ded-49ae-9b37-0eb2477da9c8', 'Caller Desk 4', '1d345f5b-1e27-4e41-92e9-62063929cecb', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e50bd5c6-bc2c-43b2-914c-96c5d5abe109', 'Caller Desk 5', '1d345f5b-1e27-4e41-92e9-62063929cecb', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('64024b46-5435-44ff-9b57-82f61342b74e', 'Caller Desk 6', '1d345f5b-1e27-4e41-92e9-62063929cecb', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0d66ef2b-9dd4-4482-b174-4e16c5797586', 'Left Desk', '8e7e1a2c-b1af-4702-b410-8d9d36e622af', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('ba2e27ef-8b4c-40dc-9bcb-64f524465891', 'Right Desk', '8e7e1a2c-b1af-4702-b410-8d9d36e622af', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('d37a340c-d36f-4d34-8f56-1baaabf5c622', 'Front Desk', '8e7e1a2c-b1af-4702-b410-8d9d36e622af', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('951b0f74-9081-47a1-9412-df906266b93f', 'Back Desk', '8e7e1a2c-b1af-4702-b410-8d9d36e622af', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('abbd9075-69c7-49fa-97d7-38f3ec094789', 'Right Desk', 'ee86f122-0614-40b6-8a74-888c853bf370', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('d5a1bee4-76a9-43f7-b12b-deabdf4c694c', 'Left Desk', 'ee86f122-0614-40b6-8a74-888c853bf370', 'b0a60efc-48b0-4bbc-ba6e-c0c2572c1608', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('da5f3bbd-fcba-4f43-b144-aeb5abbfb4b6', 'Desk 1', '729fb62a-8220-40e6-8a91-7b12ea18b6dd', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('000d5af7-e8f3-469e-8ab0-84241184c87e', 'Desk 2', '729fb62a-8220-40e6-8a91-7b12ea18b6dd', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('57c53a2e-5954-412e-a297-40c7f2c7fda1', 'Desk 3', '729fb62a-8220-40e6-8a91-7b12ea18b6dd', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e94a9df1-321a-49ec-8202-04c00133df01', 'Desk 4', '729fb62a-8220-40e6-8a91-7b12ea18b6dd', '251a0802-2d7d-43f4-8b43-d661131bfd53', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('da97a560-9db8-4482-bd90-66cf17a1530a', 'Desk 1', '9a31d011-d739-4277-bb3e-7f910650b3d7', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('739dd655-3ff5-43c4-9b00-18bb0e35c715', 'Desk 2', '9a31d011-d739-4277-bb3e-7f910650b3d7', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('1291d1e6-ae0b-477c-ac86-0a2173dd0549', 'Desk 3', '9a31d011-d739-4277-bb3e-7f910650b3d7', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('1163f718-b2fe-47f3-91aa-59207ba29630', 'Desk 4', '9a31d011-d739-4277-bb3e-7f910650b3d7', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('75e5c0b8-48c7-4c23-ac40-2ba7a59da805', 'Desk 1', 'e3389959-c161-4e8f-a3d5-d7919adbcbaf', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('44a71873-519e-42b2-a217-69f0cc9786b0', 'Desk 2', 'e3389959-c161-4e8f-a3d5-d7919adbcbaf', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('58a2f932-7e30-4f6b-ae84-ce29454bb705', 'Desk 3', 'e3389959-c161-4e8f-a3d5-d7919adbcbaf', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('c5d8c7fa-6e0a-4289-827b-d283924cc86b', 'Desk 4', 'e3389959-c161-4e8f-a3d5-d7919adbcbaf', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('1176cc7b-9efe-4a66-9d2f-6015b3bef5ab', 'Desk 1', '7c5351ca-150e-40e2-9369-e6c245c37d2c', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('8f26a51a-a90e-443a-844d-98c9519324e7', 'Desk 2', '7c5351ca-150e-40e2-9369-e6c245c37d2c', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('3475d9d9-c0f8-4d5a-b172-ec05ddbd6492', 'Desk 3', '7c5351ca-150e-40e2-9369-e6c245c37d2c', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e39be52a-4910-408b-9d59-c9bb1e1ace5c', 'Desk 4', '7c5351ca-150e-40e2-9369-e6c245c37d2c', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('a2cd483f-b1b5-454e-bc62-d16adba409b2', 'Desk 1', '6e9a2400-4f87-41d9-ad34-5de909986130', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('b33ad35e-961f-46d5-b71a-94c2e729e888', 'Desk 2', '6e9a2400-4f87-41d9-ad34-5de909986130', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('2faff67d-2429-4a13-bb6b-86eb1ba28051', 'Desk 3', '6e9a2400-4f87-41d9-ad34-5de909986130', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('c82cc29e-1b96-41f3-8b6e-4a0a56658106', 'Desk 4', '6e9a2400-4f87-41d9-ad34-5de909986130', '8ab4e9e6-e412-4dd5-b099-e5b6e5a9512a', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0d9f06b4-39fe-4333-b8d9-c40a91d3d071', 'Desk 1', '928f46b8-d74d-4a6a-b517-e0846806fe1f', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('674b78f5-d7ca-4648-aea9-67f84dcca960', 'Desk 2', '928f46b8-d74d-4a6a-b517-e0846806fe1f', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e21bf77d-025a-441f-aa68-060f79fb6310', 'Desk 3', '928f46b8-d74d-4a6a-b517-e0846806fe1f', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('b669f701-6feb-4319-848f-5fb1248cf0b1', 'Desk 4', '928f46b8-d74d-4a6a-b517-e0846806fe1f', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0b0f28f5-981d-4e01-808f-0b4abf7091a5', 'Desk 1', '84b76c97-d7f9-4529-805b-494195e37448', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('eab2373f-f100-41a9-84df-81de5fef0484', 'Desk 2', '84b76c97-d7f9-4529-805b-494195e37448', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('f0f93c66-05ae-48ff-aee1-e2b293b4c868', 'Desk 3', '84b76c97-d7f9-4529-805b-494195e37448', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('0173269d-6f19-4f2f-b4ee-f42b8845666e', 'Desk 4', '84b76c97-d7f9-4529-805b-494195e37448', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('7e6bde39-b4ac-488b-9144-935c97aa80a9', 'Desk 1', '2d1e3874-f815-4b26-8ba1-ddf4e86abb48', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('a61df82b-120d-49dc-8389-fe6b269042e3', 'Desk 2', '2d1e3874-f815-4b26-8ba1-ddf4e86abb48', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('c2863823-b0cc-4ed7-a480-f7b5cb463f89', 'Desk 3', '2d1e3874-f815-4b26-8ba1-ddf4e86abb48', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('fa40b2fc-44ca-47bf-865d-5040867322ec', 'Desk 4', '2d1e3874-f815-4b26-8ba1-ddf4e86abb48', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('c4abf0d9-9464-4d87-9b12-e56cc537628f', 'Desk 1', '54176838-810c-4332-8e04-4ed411ed5091', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('fc33fa7c-35ac-4317-ae71-09c4867fdc69', 'Desk 2', '54176838-810c-4332-8e04-4ed411ed5091', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('2889bd84-d853-4774-ad42-d0d0e60b7910', 'Desk 3', '54176838-810c-4332-8e04-4ed411ed5091', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e6f95dd0-32f0-4aba-a561-0cce830147b2', 'Desk 4', '54176838-810c-4332-8e04-4ed411ed5091', 'a8ac0d24-ecdb-4a15-b867-5e26407c59f2', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('11c2f999-9f83-4161-b9a2-1ada5a041413', 'Desk 1', '29918eb4-f4ae-44fb-b1a1-76050a04c403', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('27a4c1d5-2cef-473b-9be7-6a5a1c309e29', 'Desk 2', '29918eb4-f4ae-44fb-b1a1-76050a04c403', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('9b3f8913-3d9f-4e4c-aa4c-0ca1bb06c4ec', 'Desk 3', '29918eb4-f4ae-44fb-b1a1-76050a04c403', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('2db5ba7f-d972-42e7-8100-68749aad118c', 'Desk 4', '29918eb4-f4ae-44fb-b1a1-76050a04c403', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('1b2df830-12f7-4eba-a397-4d02b382d141', 'Desk 1', '0129358e-515a-4d31-a531-59da6134ff66', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('4476e111-2155-4965-a084-41647554043c', 'Desk 2', '0129358e-515a-4d31-a531-59da6134ff66', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('7aa5e7b1-1c9c-4e8b-885a-aceda7db8753', 'Desk 3', '0129358e-515a-4d31-a531-59da6134ff66', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('e0ccd6a6-5d1d-4182-92c0-53f4f8b1d5c3', 'Desk 4', '0129358e-515a-4d31-a531-59da6134ff66', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('20bdf574-9a3a-4acc-8450-4d04157df04c', 'Desk 1', 'bbde0c96-5ca2-44bd-931e-6b6624187781', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('3a65cee0-036d-4323-a40d-98e98f13b5b2', 'Desk 2', 'bbde0c96-5ca2-44bd-931e-6b6624187781', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('9f582b8f-3b93-4c0a-ad0b-f2bdf71eeed9', 'Desk 3', 'bbde0c96-5ca2-44bd-931e-6b6624187781', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('c2aa1cd1-296a-432f-97d5-f900f17a2ca1', 'Desk 4', 'bbde0c96-5ca2-44bd-931e-6b6624187781', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('31bbe5f6-025d-4a77-8f7c-c11f2f342f50', 'Desk 1', '17194565-4192-4c6e-ae98-dc4bdb54ea05', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('4765f53c-f63c-40e9-a51e-fa52ff2cbe2f', 'Desk 2', '17194565-4192-4c6e-ae98-dc4bdb54ea05', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('3624f463-ee8a-4d71-a747-20f667834b0a', 'Desk 3', '17194565-4192-4c6e-ae98-dc4bdb54ea05', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);
INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID", "IsMarkedForDeletion") VALUES ('b6b86a66-0718-4822-a334-ad7c23447f58', 'Desk 4', '17194565-4192-4c6e-ae98-dc4bdb54ea05', '2a666a5f-a5f7-4d55-9cab-118c602eacbc', false);


--
-- Data for Name: User; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('8f351169-8894-42a3-bcf7-237d1b26ada4', 'Bob', 'Employee', 'bob.employee@acme.com', 'AQAAAAEAACcQAAAAEJJc6vH2kn2sihVl9NaXPPtEhVBptu4NHrB5sWyf4nSjMAZAD7u8tYoOk7Y18QFlNA==', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('55696655-4a02-4aa5-80ad-6d25566c786a', 'Armin', 'Schneider', 'armin.schneider@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', false, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('11cbf9ad-97a7-4354-b6d2-568f445c6aae', 'Shiva', 'Bueno', 'shiva.bueno@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('f7bd6f02-76f1-4e3e-8dba-49ea6aa0b7d7', 'Michelle', 'Sarkozi', 'michelle.sarkozi@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('aac519d5-584e-4baf-b591-b92090d86020', 'Raul', 'Wegner', 'raul.wegner@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('22dd61ac-b7bd-45e6-bba8-341088c11998', 'Alice', 'Admin', 'alice.admin@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', true, true, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('b3c05154-0ed9-4f0a-a156-1ceafae68631', 'Alicia', 'Connery', 'alicia.connery@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('04d11d03-9fb5-4de5-9734-952929c8787f', 'Jane', 'Hernandez', 'jane.hernandez@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, true, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('5fa5c6cb-fe3d-44ae-9d3d-b5bb5bee21d5', 'Harry', 'Potter', 'harry.potter@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, true, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('ea58b3a2-7603-4f5b-84e1-fb64e311eb00', 'Nils', 'Andreasen', 'nils.andreasen@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('c023c2b5-704e-48ed-aab5-440084219436', 'Hannah', 'Madison', 'hannah.madison@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('6d4a8ec2-d916-4d8d-a2a5-5bea75aec09f', 'Samuel', 'Wilbur', 'samuel.wilbur@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('40e2c613-6616-428f-a48d-c4c01da9dd79', 'Elvira', 'Gruber', 'elvira.gruber@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('5f96b64d-a4e9-4986-b6fd-b263f917cb8f', 'Jonathan', 'Martinson', 'jonathan.martinson@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('34a67080-e3ef-46fa-a97d-8f294983265c', 'FAU', 'Admin', 'fau.admin@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '5e2761c1-86ae-4cbe-8b40-c1251ee38777', true, true, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('f94fbc1e-3fec-4939-bc50-2abfff0616cd', 'TU Berlin', 'Admin', 'tu.admin@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', 'eb6aa331-ad47-4f25-8a4a-1a9a40399da5', true, true, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('c7e950bf-3f5f-4493-8b41-e5c650b3f285', 'FU Berlin', 'Admin', 'fu.admin@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '6fb5df25-64b1-4986-b072-04a5f1285b80', true, true, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('97b670b9-788a-484c-b2a7-325f18ccb6b4', 'Alice', 'Liddell', 'alice.liddell@sola.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '02c03441-5caa-4a7e-a710-b64cf62cc7aa', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('1ba26de3-b938-41de-8553-35bffe6730d6', 'Ernst', 'Reuter', 'ernst.reuter@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '6fb5df25-64b1-4986-b072-04a5f1285b80', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('876d811e-9b1d-49da-a5ac-4102c6f81067', 'Friedrich', 'von Brandenburg-Bayreuth', 'friedrich@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', '5e2761c1-86ae-4cbe-8b40-c1251ee38777', true, false, false);
INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin", "IsMarkedForDeletion") VALUES ('258624a7-ddc7-494f-877a-22f0a7e068cb', 'Wilhelm II.', 'von Preußen', 'wilhelm@acme.com', 'AQAAAAEAACcQAAAAENqF209NJBIx/ViALpcydCj0TmQGXFeS9jMkfj7bLuAIO9vMydo/guHCXHa7zhDg4w==', 'eb6aa331-ad47-4f25-8a4a-1a9a40399da5', true, false, false);


--
-- Data for Name: Booking; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('a0dde68e-78e0-419a-832f-070ead717070', '8f351169-8894-42a3-bcf7-237d1b26ada4', '6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', '2022-11-21 10:26:39.486746', '2022-12-09 09:00:00', '2022-12-09 18:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('00cb86b7-0c5f-4c54-a7b8-4cc38cd1b237', '8f351169-8894-42a3-bcf7-237d1b26ada4', '6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', '2022-11-21 10:26:39.486746', '2023-01-21 09:00:00', '2023-01-21 18:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('095aa7e9-c728-42e2-bc5e-79612861785f', '8f351169-8894-42a3-bcf7-237d1b26ada4', '223386c1-8c71-4f6e-92b0-d5a54e2d268b', '2022-11-21 10:24:13.330898', '2023-01-22 09:00:00', '2023-01-22 18:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('1a68b921-f19c-41b1-8f66-002b5ef90bcd', '8f351169-8894-42a3-bcf7-237d1b26ada4', '6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', '2022-11-21 10:26:39.486746', '2023-01-24 09:00:00', '2023-01-24 18:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('68ce5d49-5e09-4e9d-a489-2740d234a613', '8f351169-8894-42a3-bcf7-237d1b26ada4', '223386c1-8c71-4f6e-92b0-d5a54e2d268b', '2022-11-21 10:24:54.156324', '2023-01-23 09:00:00', '2023-01-23 18:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('f32e9f9d-28c2-45c8-a0a8-1eca21b57143', '8f351169-8894-42a3-bcf7-237d1b26ada4', '81979b01-37a0-4377-a13b-785f81304271', '2022-11-21 10:26:39.486746', '2023-01-25 09:00:00', '2023-01-25 13:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('9515408e-4742-426d-a295-1a2d80038849', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-01-28 18:16:31', '2023-01-30 09:00:00', '2023-01-30 17:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('4db81875-1fbd-44a0-beb7-ae8ca9ace01b', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-01-28 18:16:34', '2023-01-31 09:00:00', '2023-01-31 17:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('5cb1ba8b-610d-4fed-9c00-6d2372afdfd0', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-01-28 18:16:36', '2023-02-01 09:00:00', '2023-02-01 17:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('1757b5a4-d5c1-4151-bbd1-77432fb32b21', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-01-28 18:16:37', '2023-02-02 09:00:00', '2023-02-02 17:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('d80022e3-dcb0-4a62-85a0-534d64040c5d', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-01-28 18:16:39', '2023-02-03 09:00:00', '2023-02-03 17:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('d816d88b-1c68-417b-9f36-ad25f4c2570d', '22dd61ac-b7bd-45e6-bba8-341088c11998', '330f037c-2381-4ec5-abca-4cb7ddbe7332', '2023-01-31 16:37:14.724314', '2023-02-01 07:00:00', '2023-02-01 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('3642946c-f1a8-4878-a4c5-6c584af2f06d', '22dd61ac-b7bd-45e6-bba8-341088c11998', '330f037c-2381-4ec5-abca-4cb7ddbe7332', '2023-01-31 16:44:06.657388', '2023-02-23 07:00:00', '2023-02-23 08:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('80e12bab-a3e8-4c08-a051-0007109397c7', '22dd61ac-b7bd-45e6-bba8-341088c11998', '527c3b1a-8fdf-4e07-99ec-95b7ed7041db', '2023-01-31 16:44:09.158909', '2023-03-02 07:00:00', '2023-03-02 08:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('18505bbb-1ac7-4ceb-b06b-cc4c8881d17b', '22dd61ac-b7bd-45e6-bba8-341088c11998', '52b74553-a686-4b6e-8bda-07e17f5a78b5', '2023-01-31 16:48:48.559925', '2023-02-01 07:00:00', '2023-02-01 15:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('4bad0835-1bc8-4bf7-a020-2badce9b97ee', '876d811e-9b1d-49da-a5ac-4102c6f81067', '58a2f932-7e30-4f6b-ae84-ce29454bb705', '2023-02-05 14:28:28.951078', '2023-02-09 07:00:00', '2023-02-09 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('6a831dce-fd45-4afb-a347-9d7f8e1eae37', '876d811e-9b1d-49da-a5ac-4102c6f81067', '58a2f932-7e30-4f6b-ae84-ce29454bb705', '2023-02-05 14:28:40.19284', '2023-02-10 07:00:00', '2023-02-10 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('49aad2ae-2174-4011-a27b-b1fdd41e73d6', '876d811e-9b1d-49da-a5ac-4102c6f81067', '58a2f932-7e30-4f6b-ae84-ce29454bb705', '2023-02-05 14:28:50.473593', '2023-02-08 07:00:00', '2023-02-08 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('8693d594-6f92-47a8-8994-08fabdf5b3ef', '258624a7-ddc7-494f-877a-22f0a7e068cb', 'b669f701-6feb-4319-848f-5fb1248cf0b1', '2023-02-05 14:30:40.206267', '2023-02-08 07:00:00', '2023-02-08 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('195bbd64-4b3a-490a-9b8e-3fe3d62ebf19', '258624a7-ddc7-494f-877a-22f0a7e068cb', 'b669f701-6feb-4319-848f-5fb1248cf0b1', '2023-02-05 14:30:49.663474', '2023-02-09 07:00:00', '2023-02-09 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('b7717f45-2a19-4a82-85ab-07785a5e79db', '258624a7-ddc7-494f-877a-22f0a7e068cb', 'b669f701-6feb-4319-848f-5fb1248cf0b1', '2023-02-05 14:30:59.099279', '2023-02-10 07:00:00', '2023-02-10 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('951ef162-fda1-4136-9bcd-3150e43ce424', '1ba26de3-b938-41de-8553-35bffe6730d6', '31bbe5f6-025d-4a77-8f7c-c11f2f342f50', '2023-02-05 14:31:50.046957', '2023-02-08 07:00:00', '2023-02-08 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('c9bb31e5-cf9b-4e5a-afd4-5948cded5a0d', '1ba26de3-b938-41de-8553-35bffe6730d6', '31bbe5f6-025d-4a77-8f7c-c11f2f342f50', '2023-02-05 14:32:01.424831', '2023-02-09 07:00:00', '2023-02-09 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('8f41823e-856c-414b-bf18-5a5c3eea44fd', '1ba26de3-b938-41de-8553-35bffe6730d6', '31bbe5f6-025d-4a77-8f7c-c11f2f342f50', '2023-02-05 14:32:10.052421', '2023-02-10 07:00:00', '2023-02-10 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('c1fbe634-ed00-4cb3-9642-d63c258c39bc', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-02-05 14:36:19.167951', '2023-02-09 07:00:00', '2023-02-09 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('f1e057df-8cea-46e8-a1d2-7c50075846c0', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-02-05 14:36:25.564751', '2023-02-10 07:00:00', '2023-02-10 16:00:00');
INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES ('f28b423d-ae39-4c06-9a06-8886c63102f9', '04d11d03-9fb5-4de5-9734-952929c8787f', '1af9a523-e3e3-4837-a425-444a35322e25', '2023-02-05 14:36:39.834733', '2023-02-08 07:00:00', '2023-02-08 16:00:00');


--
-- Data for Name: Role; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."Role" ("RoleID", "RoleName", "CompanyID") VALUES ('3830cb55-71ae-4579-966e-e0a543b14cf7', 'ACME Admin', '8745f064-c658-4a32-83fb-9d7d7e6d8f17');
INSERT INTO "public"."Role" ("RoleID", "RoleName", "CompanyID") VALUES ('624d98a5-1b00-46c1-9211-7fd39ea9c9f1', 'ACME Employee', '8745f064-c658-4a32-83fb-9d7d7e6d8f17');


--
-- Data for Name: UserRole; Type: TABLE DATA; Schema: "public"; Owner: postgres
--

INSERT INTO "public"."UserRole" ("RoleID", "UserID") VALUES ('3830cb55-71ae-4579-966e-e0a543b14cf7', '22dd61ac-b7bd-45e6-bba8-341088c11998');
INSERT INTO "public"."UserRole" ("RoleID", "UserID") VALUES ('624d98a5-1b00-46c1-9211-7fd39ea9c9f1', '8f351169-8894-42a3-bcf7-237d1b26ada4');


--
-- PostgreSQL database dump complete
--

