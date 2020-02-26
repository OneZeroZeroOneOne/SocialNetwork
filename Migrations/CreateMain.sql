CREATE TABLE "Attachment" (
  "Id" int8 PRIMARY KEY NOT NULL GENERATED ALWAYS AS IDENTITY,
  "ContentType" text NOT NULL,
  "Path" text NOT NULL,
  "CreateDateTime" date NOT NULL
);

CREATE TABLE "BoardType" (
  "Id" uuid PRIMARY KEY NOT NULL,
  "Description" text NOT NULL
);

CREATE TABLE "ReactionTypeComment" (
  "ReactionId" uuid PRIMARY KEY NOT NULL,
  "Content" text NOT NULL
);

CREATE TABLE "ReactionTypePost" (
  "ReactionId" uuid PRIMARY KEY NOT NULL,
  "Content" text NOT NULL
);

CREATE TABLE "Role" (
  "RoleId" uuid PRIMARY KEY NOT NULL,
  "RoleName" text NOT NULL
);

CREATE TABLE "SettingNumber" (
  "Key" text PRIMARY KEY NOT NULL,
  "Value" int4 NOT NULL,
  "CreateDateTime" date NOT NULL
);

CREATE TABLE "SettingText" (
  "Key" text PRIMARY KEY NOT NULL,
  "Value" text NOT NULL,
  "CreateDateTime" date NOT NULL
);

CREATE TABLE "User" (
  "Id" uuid PRIMARY KEY NOT NULL,
  "Name" text,
  "Email" text,
  "AvatarUrl" text,
  "DateOfBirth" timestamp
);

CREATE TABLE "Board" (
  "Id" uuid PRIMARY KEY NOT NULL,
  "BoardTypeId" uuid NOT NULL,
  "Name" text,
  "Description" text,
  "CreateDateTime" timestamp NOT NULL,
  "IsArchived" bool NOT NULL DEFAULT false
);

CREATE TABLE "Post" (
  "Id" int8 PRIMARY KEY NOT NULL GENERATED ALWAYS AS IDENTITY,
  "BoardId" uuid NOT NULL,
  "UserId" uuid NOT NULL,
  "Text" text NOT NULL,
  "Date" timestamp NOT NULL,
  "IsArchived" bool NOT NULL DEFAULT false
);

CREATE TABLE "ReactionPost" (
  "UserId" uuid NOT NULL,
  "PostId" int8 NOT NULL,
  "ReactionId" uuid NOT NULL,
  PRIMARY KEY ("ReactionId", "UserId", "PostId")
);

CREATE TABLE "UserConfirmationToken" (
  "UserId" uuid NOT NULL,
  "ConfirmEmail" text NOT NULL,
  "CreateDateTime" timestamp NOT NULL DEFAULT (now()),
  "ConfirmId" uuid PRIMARY KEY NOT NULL,
  "IsActive" bool NOT NULL DEFAULT true
);

CREATE TABLE "UserSecurity" (
  "UserId" uuid NOT NULL,
  "RoleId" uuid NOT NULL,
  "Password" text NOT NULL,
  "IsConfirmed" bool NOT NULL DEFAULT false
);

CREATE TABLE "AttachmentPost" (
  "PostId" int8 NOT NULL,
  "AttachmentId" int8 NOT NULL,
  PRIMARY KEY ("PostId", "AttachmentId")
);

CREATE TABLE "Comment" (
  "Id" int8 PRIMARY KEY NOT NULL GENERATED ALWAYS AS IDENTITY,
  "UserId" uuid NOT NULL,
  "PostId" int8 NOT NULL,
  "Text" text NOT NULL,
  "Date" timestamp NOT NULL,
  "IsArchived" bool NOT NULL DEFAULT false
);

CREATE TABLE "ReactionComment" (
  "UserId" uuid NOT NULL,
  "CommentId" int8 NOT NULL,
  "ReactionId" uuid NOT NULL,
  PRIMARY KEY ("ReactionId", "UserId", "CommentId")
);

CREATE TABLE "AttachmentComment" (
  "CommentId" int8 NOT NULL,
  "AttachmentId" int8 NOT NULL,
  PRIMARY KEY ("CommentId", "AttachmentId")
);

ALTER TABLE "ReactionPost" ADD FOREIGN KEY ("PostId") REFERENCES "Post" ("Id");

ALTER TABLE "Comment" ADD FOREIGN KEY ("PostId") REFERENCES "Post" ("Id");

ALTER TABLE "Board" ADD FOREIGN KEY ("BoardTypeId") REFERENCES "BoardType" ("Id");

ALTER TABLE "Post" ADD FOREIGN KEY ("BoardId") REFERENCES "Board" ("Id");

ALTER TABLE "Post" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "ReactionPost" ADD FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypePost" ("ReactionId");

ALTER TABLE "ReactionPost" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "UserConfirmationToken" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "UserSecurity" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "UserSecurity" ADD FOREIGN KEY ("RoleId") REFERENCES "Role" ("RoleId");

ALTER TABLE "AttachmentPost" ADD FOREIGN KEY ("PostId") REFERENCES "Post" ("Id");

ALTER TABLE "AttachmentPost" ADD FOREIGN KEY ("AttachmentId") REFERENCES "Attachment" ("Id");

ALTER TABLE "Comment" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "ReactionComment" ADD FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypeComment" ("ReactionId");

ALTER TABLE "ReactionComment" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "ReactionComment" ADD FOREIGN KEY ("CommentId") REFERENCES "Comment" ("Id");

ALTER TABLE "AttachmentComment" ADD FOREIGN KEY ("CommentId") REFERENCES "Comment" ("Id");

ALTER TABLE "AttachmentComment" ADD FOREIGN KEY ("AttachmentId") REFERENCES "Attachment" ("Id");
