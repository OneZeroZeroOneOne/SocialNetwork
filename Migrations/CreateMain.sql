-- Drop table

-- DROP TABLE public."Attachment";

CREATE SEQUENCE attachment_id_seq;
CREATE SEQUENCE post_comment_id_seq;

CREATE TABLE public."Attachment" (
	"Id" int8 NOT NULL default NEXTVAL('attachment_id_seq'),
	"ContentType" text NOT NULL,
	"Path" text NOT NULL,
	"CreateDateTime" date NOT NULL,
	"Preview" text NULL,
	"Hash" uuid NULL,
	"Width" int8 NULL,
	"Height" int8 NULL,
	"DisplayName" text NULL,
	CONSTRAINT "Attachment_pkey" PRIMARY KEY ("Id")
);

-- Drop table

-- DROP TABLE public."BoardType";

CREATE TABLE public."BoardType" (
	"Id" uuid NOT NULL,
	"Description" text NOT NULL,
	CONSTRAINT "BoardType_pkey" PRIMARY KEY ("Id")
);

-- Drop table

-- DROP TABLE public."ReactionTypeComment";

CREATE TABLE public."ReactionTypeComment" (
	"ReactionId" uuid NOT NULL,
	"Content" text NOT NULL,
	CONSTRAINT "ReactionTypeComment_pkey" PRIMARY KEY ("ReactionId")
);

-- Drop table

-- DROP TABLE public."ReactionTypePost";

CREATE TABLE public."ReactionTypePost" (
	"ReactionId" uuid NOT NULL,
	"Content" text NOT NULL,
	CONSTRAINT "ReactionTypePost_pkey" PRIMARY KEY ("ReactionId")
);

-- Drop table

-- DROP TABLE public."Role";

CREATE TABLE public."Role" (
	"RoleId" uuid NOT NULL,
	"RoleName" text NOT NULL,
	CONSTRAINT "Role_pkey" PRIMARY KEY ("RoleId")
);

-- Drop table

-- DROP TABLE public."SettingNumber";

CREATE TABLE public."SettingNumber" (
	"Key" text NOT NULL,
	"Value" int4 NOT NULL,
	"CreateDateTime" date NOT NULL,
	CONSTRAINT "SettingNumber_pkey" PRIMARY KEY ("Key")
);

-- Drop table

-- DROP TABLE public."SettingText";

CREATE TABLE public."SettingText" (
	"Key" text NOT NULL,
	"Value" text NOT NULL,
	"CreateDateTime" date NOT NULL,
	CONSTRAINT "SettingText_pkey" PRIMARY KEY ("Key")
);

-- Drop table

-- DROP TABLE public."User";

CREATE TABLE public."User" (
	"Id" uuid NOT NULL,
	"Name" text NULL,
	"Email" text NULL,
	"AvatarUrl" text NULL,
	"DateOfBirth" timestamp NULL,
	CONSTRAINT "User_pkey" PRIMARY KEY ("Id")
);

-- Drop table

-- DROP TABLE public."Board";

CREATE TABLE public."Board" (
	"Id" uuid NOT NULL,
	"BoardTypeId" uuid NOT NULL,
	"Name" text NULL,
	"Description" text NULL,
	"CreateDateTime" timestamp NOT NULL,
	"IsArchived" bool NOT NULL DEFAULT false,
	CONSTRAINT "Board_pkey" PRIMARY KEY ("Id"),
	CONSTRAINT "Board_BoardTypeId_fkey" FOREIGN KEY ("BoardTypeId") REFERENCES "BoardType"("Id")
);

-- Drop table

-- DROP TABLE public."Post";

CREATE TABLE public."Post" (
	"Id" int8 NOT NULL default NEXTVAL('post_comment_id_seq'),
	"BoardId" uuid NOT NULL,
	"UserId" uuid NOT NULL,
	"Text" text NOT NULL,
	"Date" timestamp NOT NULL,
	"IsArchived" bool NOT NULL DEFAULT false,
	"Title" text NULL,
	CONSTRAINT "Post_pkey" PRIMARY KEY ("Id"),
	CONSTRAINT "Post_BoardId_fkey" FOREIGN KEY ("BoardId") REFERENCES "Board"("Id"),
	CONSTRAINT "Post_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."ReactionPost";

CREATE TABLE public."ReactionPost" (
	"UserId" uuid NOT NULL,
	"PostId" int8 NOT NULL,
	"ReactionId" uuid NOT NULL,
	CONSTRAINT "ReactionPost_pkey" PRIMARY KEY ("ReactionId", "UserId", "PostId"),
	CONSTRAINT "ReactionPost_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id"),
	CONSTRAINT "ReactionPost_ReactionId_fkey" FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypePost"("ReactionId"),
	CONSTRAINT "ReactionPost_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."UserConfirmationToken";

CREATE TABLE public."UserConfirmationToken" (
	"UserId" uuid NOT NULL,
	"ConfirmEmail" text NOT NULL,
	"CreateDateTime" timestamp NOT NULL DEFAULT now(),
	"ConfirmId" uuid NOT NULL,
	"IsActive" bool NOT NULL DEFAULT true,
	CONSTRAINT "UserConfirmationToken_pkey" PRIMARY KEY ("ConfirmId"),
	CONSTRAINT "UserConfirmationToken_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."UserSecurity";

CREATE TABLE public."UserSecurity" (
	"UserId" uuid NOT NULL,
	"RoleId" uuid NOT NULL,
	"Password" text NOT NULL,
	"IsConfirmed" bool NOT NULL DEFAULT false,
	CONSTRAINT "UserSecurity_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES "Role"("RoleId"),
	CONSTRAINT "UserSecurity_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."AttachmentPost";

CREATE TABLE public."AttachmentPost" (
	"PostId" int8 NOT NULL,
	"AttachmentId" int8 NOT NULL,
	CONSTRAINT "AttachmentPost_pkey" PRIMARY KEY ("PostId", "AttachmentId"),
	CONSTRAINT "AttachmentPost_AttachmentId_fkey" FOREIGN KEY ("AttachmentId") REFERENCES "Attachment"("Id"),
	CONSTRAINT "AttachmentPost_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id")
);

-- Drop table

-- DROP TABLE public."Comment";

CREATE TABLE public."Comment" (
	"Id" int8 NOT NULL default NEXTVAL('post_comment_id_seq'),
	"UserId" uuid NOT NULL,
	"PostId" int8 NOT NULL,
	"Text" text NOT NULL,
	"Date" timestamp NOT NULL,
	"IsArchived" bool NOT NULL DEFAULT false,
	CONSTRAINT "Comment_pkey" PRIMARY KEY ("Id"),
	CONSTRAINT "Comment_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id"),
	CONSTRAINT "Comment_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."ReactionComment";

CREATE TABLE public."ReactionComment" (
	"UserId" uuid NOT NULL,
	"CommentId" int8 NOT NULL,
	"ReactionId" uuid NOT NULL,
	CONSTRAINT "ReactionComment_pkey" PRIMARY KEY ("ReactionId", "UserId", "CommentId"),
	CONSTRAINT "ReactionComment_CommentId_fkey" FOREIGN KEY ("CommentId") REFERENCES "Comment"("Id"),
	CONSTRAINT "ReactionComment_ReactionId_fkey" FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypeComment"("ReactionId"),
	CONSTRAINT "ReactionComment_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."AttachmentComment";

CREATE TABLE public."AttachmentComment" (
	"CommentId" int8 NOT NULL,
	"AttachmentId" int8 NOT NULL,
	CONSTRAINT "AttachmentComment_pkey" PRIMARY KEY ("CommentId", "AttachmentId"),
	CONSTRAINT "AttachmentComment_AttachmentId_fkey" FOREIGN KEY ("AttachmentId") REFERENCES "Attachment"("Id"),
	CONSTRAINT "AttachmentComment_CommentId_fkey" FOREIGN KEY ("CommentId") REFERENCES "Comment"("Id")
);
