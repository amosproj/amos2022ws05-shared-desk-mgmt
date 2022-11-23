INSERT INTO "public"."Company" ("CompanyID", "CompanyName", "Logo") VALUES
('8745f064-c658-4a32-83fb-9d7d7e6d8f17', 'ACME Ltd.', NULL);

INSERT INTO "public"."Building" ("BuildingID", "BuildingName", "CompanyID", "Location") VALUES
('3de7afbf-0289-4ba6-bada-a34353c5548a', 'Innovation Center', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', 'Platz d. Luftbrücke 5, 12101 Berlin'),
('5fcde910-ca65-4636-84dd-54bb250252cd', 'Headquarter', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', 'Panoramastraße 1A, 10178 Berlin');

INSERT INTO "public"."Floor" ("FloorID", "BuildingID", "FloorName") VALUES
('38420525-6e54-43fb-a456-fd37ef853a1e', '5fcde910-ca65-4636-84dd-54bb250252cd', '2nd Floor'),
('604f7e75-f894-459e-ac05-f5845368243b', '3de7afbf-0289-4ba6-bada-a34353c5548a', 'Ground Floor'),
('7b5944f3-98ab-49e1-82cf-3238166f7b9d', '5fcde910-ca65-4636-84dd-54bb250252cd', 'Ground Floor'),
('7f19935f-3137-41e9-aa84-e6e4d31fc374', '5fcde910-ca65-4636-84dd-54bb250252cd', '1st Floor'),
('ffec69ab-736a-424a-a650-0d4a2653c370', '3de7afbf-0289-4ba6-bada-a34353c5548a', '1st Floor');

INSERT INTO "public"."Room" ("RoomID", "FloorID", "RoomName") VALUES
('01f876b4-1f10-4163-bb4e-5f6b9fbf32e3', '604f7e75-f894-459e-ac05-f5845368243b', 'Mind Space'),
('2f68d359-550a-428f-82be-7eb895d77371', 'ffec69ab-736a-424a-a650-0d4a2653c370', 'Silent Space'),
('66d3ec54-811b-4449-bcc8-bcc270788a51', '38420525-6e54-43fb-a456-fd37ef853a1e', 'Social Space'),
('6c37ba8b-9eb1-49e1-a009-dda96788205c', '7f19935f-3137-41e9-aa84-e6e4d31fc374', '100.1'),
('80accedd-f2a8-4782-9316-03fd82430486', '7f19935f-3137-41e9-aa84-e6e4d31fc374', '100.2'),
('a70fcf62-c27e-4bbe-881d-77758a0ded27', 'ffec69ab-736a-424a-a650-0d4a2653c370', 'Call Space'),
('b2746b05-bcfd-4f93-9cee-d2a4e7d61357', '7b5944f3-98ab-49e1-82cf-3238166f7b9d', 'Open Space');

INSERT INTO "public"."DeskType" ("DeskTypeID", "DeskTypeName", "CompanyID") VALUES
('64179bd0-98ad-460a-9e8c-ddd9102a35c8', 'Height-Adjustable Desk', '8745f064-c658-4a32-83fb-9d7d7e6d8f17'),
('b6e52e9d-c508-492f-97b6-b1c5ac5a90cf', 'Standard Desk', '8745f064-c658-4a32-83fb-9d7d7e6d8f17');

INSERT INTO "public"."Desk" ("DeskID", "DeskName", "RoomID", "DeskTypeID") VALUES
('223386c1-8c71-4f6e-92b0-d5a54e2d268b', '1', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('330f037c-2381-4ec5-abca-4cb7ddbe7332', 'C', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('40c4e435-ed31-4d39-ab6a-68a21131d256', '2', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('527c3b1a-8fdf-4e07-99ec-95b7ed7041db', 'E', '66d3ec54-811b-4449-bcc8-bcc270788a51', '64179bd0-98ad-460a-9e8c-ddd9102a35c8'),
('52b74553-a686-4b6e-8bda-07e17f5a78b5', 'B', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('672c5918-6e98-4cef-8e08-94f0d58d7861', '6', '6c37ba8b-9eb1-49e1-a009-dda96788205c', '64179bd0-98ad-460a-9e8c-ddd9102a35c8'),
('6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', 'A', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('81979b01-37a0-4377-a13b-785f81304271', 'F', '66d3ec54-811b-4449-bcc8-bcc270788a51', '64179bd0-98ad-460a-9e8c-ddd9102a35c8'),
('82628286-ea9a-4587-9628-a88da14f21c2', '4', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('8b3320e1-a1cb-48a0-84ca-ce58f813b584', 'D', '66d3ec54-811b-4449-bcc8-bcc270788a51', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('ac5d015a-4c2c-401e-9d4a-b413cce53bce', '3', '6c37ba8b-9eb1-49e1-a009-dda96788205c', 'b6e52e9d-c508-492f-97b6-b1c5ac5a90cf'),
('f81e33fc-434f-4f61-990e-7532af2a325b', '5', '6c37ba8b-9eb1-49e1-a009-dda96788205c', '64179bd0-98ad-460a-9e8c-ddd9102a35c8');

INSERT INTO "public"."Role" ("RoleID", "RoleName", "CompanyID") VALUES
('3830cb55-71ae-4579-966e-e0a543b14cf7', 'ACME Admin', '8745f064-c658-4a32-83fb-9d7d7e6d8f17'),
('624d98a5-1b00-46c1-9211-7fd39ea9c9f1', 'ACME Employee', '8745f064-c658-4a32-83fb-9d7d7e6d8f17');

INSERT INTO "public"."User" ("UserID", "FirstName", "LastName", "MailAddress", "Password", "CompanyID", "IsApproved", "IsCompanyAdmin") VALUES
('22dd61ac-b7bd-45e6-bba8-341088c11998', 'Alice', 'Admin', 'alice.admin@acme.com', 'AQAAAAEAACcQAAAAEFFR5AWKv9BuCTSS1ukbxcU/qowYURncg7+z5FXCO11AhNjV5UrmKXEDEJQD8n41MA==', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', 't', 't'),
('8f351169-8894-42a3-bcf7-237d1b26ada4', 'Bob', 'Employee', 'bob.employee@acme.com', 'AQAAAAEAACcQAAAAEJJc6vH2kn2sihVl9NaXPPtEhVBptu4NHrB5sWyf4nSjMAZAD7u8tYoOk7Y18QFlNA==', '8745f064-c658-4a32-83fb-9d7d7e6d8f17', 't', 'f');

INSERT INTO "public"."UserRole" ("RoleID", "UserID") VALUES
('3830cb55-71ae-4579-966e-e0a543b14cf7', '22dd61ac-b7bd-45e6-bba8-341088c11998'),
('624d98a5-1b00-46c1-9211-7fd39ea9c9f1', '8f351169-8894-42a3-bcf7-237d1b26ada4');

INSERT INTO "public"."Booking" ("BookingID", "UserID", "DeskID", "Timestamp", "StartTime", "EndTime") VALUES
('00cb86b7-0c5f-4c54-a7b8-4cc38cd1b237', '8f351169-8894-42a3-bcf7-237d1b26ada4', '6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', '2022-11-21 10:26:39.486746', '2022-11-21 09:00:00', '2022-11-21 18:00:00'),
('095aa7e9-c728-42e2-bc5e-79612861785f', '8f351169-8894-42a3-bcf7-237d1b26ada4', '223386c1-8c71-4f6e-92b0-d5a54e2d268b', '2022-11-21 10:24:13.330898', '2022-11-22 09:00:00', '2022-11-22 18:00:00'),
('1a68b921-f19c-41b1-8f66-002b5ef90bcd', '8f351169-8894-42a3-bcf7-237d1b26ada4', '6fdd3f80-3b70-4aae-8c0b-36ccba575b2f', '2022-11-21 10:26:39.486746', '2022-11-24 09:00:00', '2022-11-24 18:00:00'),
('68ce5d49-5e09-4e9d-a489-2740d234a613', '8f351169-8894-42a3-bcf7-237d1b26ada4', '223386c1-8c71-4f6e-92b0-d5a54e2d268b', '2022-11-21 10:24:54.156324', '2022-11-23 09:00:00', '2022-11-23 18:00:00'),
('f32e9f9d-28c2-45c8-a0a8-1eca21b57143', '8f351169-8894-42a3-bcf7-237d1b26ada4', '81979b01-37a0-4377-a13b-785f81304271', '2022-11-21 10:26:39.486746', '2022-11-25 09:00:00', '2022-11-25 13:00:00');
