-- Drop table

-- DROP TABLE public."Post";

CREATE TABLE public."Post" (
	"Id" uuid NOT NULL,
	"Text" text NOT NULL,
	"Date" timestamp NOT NULL,
	"UserId" uuid NOT NULL,
	CONSTRAINT "Post_pk" PRIMARY KEY ("Id"),
	CONSTRAINT "Post_fk0" FOREIGN KEY ("UserId") REFERENCES "User"("Id")
);

-- Drop table

-- DROP TABLE public."Comment";

CREATE TABLE public."Comment" (
	"Id" uuid NOT NULL,
	"Text" text NOT NULL,
	"Date" timestamp NOT NULL,
	"UserId" uuid NOT NULL,
	"PostId" uuid NOT NULL,
	CONSTRAINT "Comment_pk" PRIMARY KEY ("Id"),
	CONSTRAINT "Comment_fk0" FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
	CONSTRAINT "Comment_fk1" FOREIGN KEY ("PostId") REFERENCES "Post"("Id")
);
