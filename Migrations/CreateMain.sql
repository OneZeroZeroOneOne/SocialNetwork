-- Drop table

-- DROP TABLE public."GroupType";

CREATE TABLE public."GroupType" (
	"Id" uuid NOT NULL,
	"Description" text NOT NULL,
	CONSTRAINT "GroupType_pkey" PRIMARY KEY ("Id")
);

-- Drop table

-- DROP TABLE public."ReactionTypeComment";

CREATE TABLE public."ReactionTypeComment" (
	"ReactionId" uuid NOT NULL,
	"Content" text NOT NULL,
	CONSTRAINT "ReactionTypeComment_pk" PRIMARY KEY ("ReactionId")
);

-- Drop table

-- DROP TABLE public."ReactionTypePost";

CREATE TABLE public."ReactionTypePost" (
	"ReactionId" uuid NOT NULL,
	"Content" text NOT NULL,
	CONSTRAINT "ReactionTypePost_pk" PRIMARY KEY ("ReactionId")
);

-- Drop table

-- DROP TABLE public."Role";

CREATE TABLE public."Role" (
	"RoleId" uuid NOT NULL,
	"RoleName" text NOT NULL,
	CONSTRAINT "Role_pkey" PRIMARY KEY ("RoleId")
);

-- Drop table

-- DROP TABLE public."User";

CREATE TABLE public."User" (
	"Id" uuid NOT NULL,
	"Name" text NULL,
	"Email" text NULL,
	"AvatarUrl" text NULL,
	"DateOfBirth" timestamp NULL,
	CONSTRAINT "User_pk" PRIMARY KEY ("Id")
);

-- Drop table

-- DROP TABLE public."Group";

CREATE TABLE public."Group" (
	"Id" uuid NOT NULL,
	"GroupTypeId" uuid NOT NULL,
	"CreateDateTime" timestamp NOT NULL,
	"IsArchived" bool NOT NULL DEFAULT false,
	CONSTRAINT "Group_pkey" PRIMARY KEY ("Id"),
	CONSTRAINT "Group_GroupTypeId_fkey" FOREIGN KEY ("GroupTypeId") REFERENCES "GroupType"("Id")
);

-- Drop table

-- DROP TABLE public."Post";

CREATE TABLE public."Post" (
	"Id" uuid NOT NULL,
	"Text" text NOT NULL,
	"Date" timestamp NOT NULL,
	"UserId" uuid NOT NULL,
	"IsArchived" varchar NULL DEFAULT false,
	CONSTRAINT "Post_pk" PRIMARY KEY ("Id"),
	CONSTRAINT "Post_fk0" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."ReactionPost";

CREATE TABLE public."ReactionPost" (
	"ReactionId" uuid NOT NULL,
	"UserId" uuid NOT NULL,
	"PostId" uuid NOT NULL,
	CONSTRAINT reactionpost_pk PRIMARY KEY ("ReactionId", "UserId", "PostId"),
	CONSTRAINT "ReactionPost_fk0" FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypePost"("ReactionId"),
	CONSTRAINT "ReactionPost_fk1" FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
	CONSTRAINT "ReactionToPost_fk0" FOREIGN KEY ("PostId") REFERENCES "Post"("Id")
);

-- Drop table

-- DROP TABLE public."UserConfirmationToken";

CREATE TABLE public."UserConfirmationToken" (
	"UserId" uuid NOT NULL,
	"ConfirmEmail" text NOT NULL,
	"CreateDateTime" timestamp NOT NULL DEFAULT now(),
	"ConfirmId" uuid NOT NULL,
	"IsActive" bool NOT NULL DEFAULT true,
	CONSTRAINT userconfirmationtoken_pk PRIMARY KEY ("ConfirmId"),
	CONSTRAINT "UserConfirmationToken_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."UserGroup";

CREATE TABLE public."UserGroup" (
	"GroupId" uuid NOT NULL,
	"UserId" uuid NOT NULL,
	CONSTRAINT "UserGroup_pkey" PRIMARY KEY ("GroupId", "UserId"),
	CONSTRAINT "UserGroup_GroupId_fkey" FOREIGN KEY ("GroupId") REFERENCES "Group"("Id"),
	CONSTRAINT "UserGroup_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."UserSecurity";

CREATE TABLE public."UserSecurity" (
	"UserId" uuid NOT NULL,
	"Password" text NOT NULL,
	"RoleId" uuid NOT NULL,
	"IsConfirmed" bool NOT NULL DEFAULT false,
	CONSTRAINT "UserSecurity_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES "Role"("RoleId"),
	CONSTRAINT "UserSecurity_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."Comment";

CREATE TABLE public."Comment" (
	"Id" uuid NOT NULL,
	"Text" text NOT NULL,
	"Date" timestamp NOT NULL,
	"UserId" uuid NOT NULL,
	"PostId" uuid NOT NULL,
	"IsArchived" bool NULL DEFAULT false,
	CONSTRAINT "Comment_pk" PRIMARY KEY ("Id"),
	CONSTRAINT "Comment_fk0" FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
	CONSTRAINT "Comment_fk1" FOREIGN KEY ("PostId") REFERENCES "Post"("Id")
);

-- Drop table

-- DROP TABLE public."GroupPost";

CREATE TABLE public."GroupPost" (
	"PostId" uuid NOT NULL,
	"GroupId" uuid NOT NULL,
	CONSTRAINT "GroupPost_pkey" PRIMARY KEY ("PostId", "GroupId"),
	CONSTRAINT "GroupPost_GroupId_fkey" FOREIGN KEY ("GroupId") REFERENCES "Group"("Id"),
	CONSTRAINT "GroupPost_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id")
);

-- Drop table

-- DROP TABLE public."ReactionComment";

CREATE TABLE public."ReactionComment" (
	"ReactionId" uuid NOT NULL,
	"UserId" uuid NOT NULL,
	"CommentId" uuid NOT NULL,
	CONSTRAINT reactioncomment_pk PRIMARY KEY ("ReactionId", "UserId", "CommentId"),
	CONSTRAINT "ReactionComment_fk0" FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypeComment"("ReactionId"),
	CONSTRAINT "ReactionComment_fk1" FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
	CONSTRAINT "ReactionToComment_fk0" FOREIGN KEY ("CommentId") REFERENCES "Comment"("Id")
);


-- Drop table

-- DROP TABLE public."Attachment";

CREATE TABLE public."Attachment" (
	"Id" uuid NOT NULL,
	"ContentType" text NOT NULL,
	"Path" text NOT NULL,
	"CreateDateTime" date NOT NULL,
	CONSTRAINT "Attachment_pkey" PRIMARY KEY ("Id")
);

-- Drop table

-- DROP TABLE public."AttachmentComment";

CREATE TABLE public."AttachmentComment" (
	"CommentId" uuid NOT NULL,
	"AttachmentId" uuid NOT NULL,
	CONSTRAINT "AttachmentComment_pkey" PRIMARY KEY ("CommentId", "AttachmentId")
);

-- Drop table

-- DROP TABLE public."AttachmentPost";

CREATE TABLE public."AttachmentPost" (
	"PostId" uuid NOT NULL,
	"AttachmentId" uuid NOT NULL,
	CONSTRAINT "AttachmentPost_pkey" PRIMARY KEY ("PostId", "AttachmentId")
);

ALTER TABLE public."AttachmentComment" ADD CONSTRAINT "AttachmentComment_AttachmentId_fkey" FOREIGN KEY ("AttachmentId") REFERENCES "Attachment"("Id");
ALTER TABLE public."AttachmentComment" ADD CONSTRAINT "AttachmentComment_CommentId_fkey" FOREIGN KEY ("CommentId") REFERENCES "Comment"("Id");

ALTER TABLE public."AttachmentPost" ADD CONSTRAINT "AttachmentPost_AttachmentId_fkey" FOREIGN KEY ("AttachmentId") REFERENCES "Attachment"("Id");
ALTER TABLE public."AttachmentPost" ADD CONSTRAINT "AttachmentPost_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id");