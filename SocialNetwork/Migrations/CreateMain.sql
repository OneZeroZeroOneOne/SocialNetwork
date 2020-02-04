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

-- DROP TABLE public."UserConfirmationToken";

CREATE TABLE public."UserConfirmationToken" (
	"UserId" uuid NOT NULL,
	"ConfirmEmail" text NOT NULL,
	"CreateDateTime" timestamp NOT NULL DEFAULT now(),
	"ConfirmId" uuid NOT NULL,
	"IsActive" bool NOT NULL DEFAULT true,
	CONSTRAINT userconfirmationtoken_pk PRIMARY KEY ("ConfirmId")
);

-- Drop table

-- DROP TABLE public."UserSecurity";

CREATE TABLE public."UserSecurity" (
	"UserId" uuid NOT NULL,
	"Password" text NOT NULL,
	"Role" text NOT NULL,
	"IsConfirmed" bool NOT NULL DEFAULT false
);
