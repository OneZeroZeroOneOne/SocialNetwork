INSERT INTO public."Role" ("RoleId","RoleName") VALUES 
('75e773ee-f0b7-4dc5-bb07-04dfd40fa4e6','Admin')
,('0967a981-a365-4553-83c2-fe08b4aee941','Member')
,('49dd3f01-d0db-44ae-8dcd-deaf76a5520f','PreMember');

INSERT INTO public."User" ("Id","Name","Email","AvatarUrl","DateOfBirth") VALUES 
('ae18ad56-6ddb-4630-9f13-1a3196f8b75b',NULL,'admin',NULL,NULL)
,('37f56076-4eff-400d-8ba0-0c8b37491182','Anonim','anonim',NULL,NULL)
;

INSERT INTO public."UserSecurity" ("UserId","RoleId","Password","IsConfirmed") VALUES 
('ae18ad56-6ddb-4630-9f13-1a3196f8b75b','75e773ee-f0b7-4dc5-bb07-04dfd40fa4e6','1000.FjXD2g8E2MGm+GihRY34AA==.AZ+OQWDYIxXs1H2tg2Tf+8VyisR4cVaz2xJfJ8rBEOg=',false)
,('37f56076-4eff-400d-8ba0-0c8b37491182','0967a981-a365-4553-83c2-fe08b4aee941','1000.FjXD2g8E2MGm+GihRY34AA==.AZ+OQWDYIxXs1H2tg2Tf+8VyisR4cVaz2xJfJ8rBEOg=',false)
;

INSERT INTO public."UserConfirmationToken" ("UserId","ConfirmEmail","CreateDateTime","ConfirmId","IsActive") VALUES 
('ae18ad56-6ddb-4630-9f13-1a3196f8b75b','admin','2020-02-26 13:31:38.754','498a27df-cd55-4dfd-b218-db3860c7a1be',true)
;

INSERT INTO public."BoardType" ("Id","Description") VALUES 
('20ca5862-9a97-4e09-afac-dae577ae0eb0','public');

INSERT INTO public."Board" ("Id","BoardTypeId","Name","Description","CreateDateTime","IsArchived") VALUES 
('bcf58d82-d9e7-4c11-8645-a564b8ee6f7b','20ca5862-9a97-4e09-afac-dae577ae0eb0','b','Бред','2020-02-26 15:29:10.000',false)
,('91213a0a-c3ca-4804-8432-fd5202c24705','20ca5862-9a97-4e09-afac-dae577ae0eb0','a','Аниме','2020-02-26 15:29:10.000',false);

INSERT INTO public."SettingText" ("Key","Value","CreateDateTime") VALUES 
('AllowedHtmlTags','p,i,blockquote,b,code,col,em,pre,tr,strike,span,green,sp,br','2020-02-28')
;
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(1, 'bcf58d82-d9e7-4c11-8645-a564b8ee6f7b', 'ImagePreview', 'http://img0.joyreactor.cc/pics/post/art-%D0%BE%D0%BB%D0%B4%D1%84%D0%B0%D0%B3-2ch-4699278.jpeg');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(5, '10ed4c5c-5603-4fc9-9c6c-07021158a0ba', 'ImagePreview', 'https://2ch.pm/fag/src/10584264/15893552599724.jpg');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(6, '10ed4c5c-5603-4fc9-9c6c-07021158a0ba', 'ImagePreview', 'https://2ch.pm/fag/src/10564833/15891783782510.jpg');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(7, 'dba53a91-ba85-4a35-a26b-2db36f8f7888', 'ImagePreview', 'https://2ch.hk/vg/src/33885825/15892286414823.jpg');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(8, 'dba53a91-ba85-4a35-a26b-2db36f8f7888', 'ImagePreview', 'https://i.pinimg.com/originals/be/69/43/be6943da11f667727eb416420d67d3c3.jpg');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(2, 'bcf58d82-d9e7-4c11-8645-a564b8ee6f7b', 'ImagePreview', 'https://2ch.hk/char/src/2777/14717699796150.jpg');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(3, '91213a0a-c3ca-4804-8432-fd5202c24705', 'ImagePreview', 'https://anime-chan.me/uploads/posts/2020-01/1578479313_78768467_p0.png');
INSERT INTO public."BoardSetting"
("SettingId", "BoardId", "Name", "Value")
VALUES(4, '91213a0a-c3ca-4804-8432-fd5202c24705', 'ImagePreview', 'https://image.winudf.com/v2/image/Y29tLmtpbmdrdXN0dXIuYXJ0bmFydXRvd2FsbHBhcGVyX3NjcmVlbl8wXzE1MTc2NzUxNzZfMDY3/screen-0.jpg?fakeurl=1&type=.jpg');

