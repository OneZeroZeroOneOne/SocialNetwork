INSERT INTO public."Role" ("RoleId","RoleName") VALUES 
('75e773ee-f0b7-4dc5-bb07-04dfd40fa4e6','Admin')
,('0967a981-a365-4553-83c2-fe08b4aee941','Member')
,('49dd3f01-d0db-44ae-8dcd-deaf76a5520f','PreMember');

INSERT INTO public."User" ("Id","Name","Email","AvatarUrl","DateOfBirth") VALUES 
('ae18ad56-6ddb-4630-9f13-1a3196f8b75b',NULL,'admin',NULL,NULL);

INSERT INTO public."UserSecurity" ("UserId","RoleId","Password","IsConfirmed") VALUES 
('ae18ad56-6ddb-4630-9f13-1a3196f8b75b','75e773ee-f0b7-4dc5-bb07-04dfd40fa4e6','1000.FjXD2g8E2MGm+GihRY34AA==.AZ+OQWDYIxXs1H2tg2Tf+8VyisR4cVaz2xJfJ8rBEOg=',false);

INSERT INTO public."UserConfirmationToken" ("UserId","ConfirmEmail","CreateDateTime","ConfirmId","IsActive") VALUES 
('ae18ad56-6ddb-4630-9f13-1a3196f8b75b','admin','2020-02-26 13:31:38.754','498a27df-cd55-4dfd-b218-db3860c7a1be',true);

INSERT INTO public."BoardType" ("Id","Description") VALUES 
('20ca5862-9a97-4e09-afac-dae577ae0eb0','public');

INSERT INTO public."Board" ("Id","BoardTypeId","Name","Description","CreateDateTime","IsArchived") VALUES 
('bcf58d82-d9e7-4c11-8645-a564b8ee6f7b','20ca5862-9a97-4e09-afac-dae577ae0eb0','b','Бред','2020-02-26 15:29:10.000',false)
,('91213a0a-c3ca-4804-8432-fd5202c24705','20ca5862-9a97-4e09-afac-dae577ae0eb0','a','Аниме','2020-02-26 15:29:10.000',false);
