CREATE TABLE "ReactionTypeComment" (
  "ReactionId" uuid NOT NULL,
  "Content" TEXT NOT NULL,
  CONSTRAINT "ReactionTypeComment_pk" PRIMARY KEY ("ReactionId")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "ReactionTypePost" (
  "ReactionId" uuid NOT NULL,
  "Content" TEXT NOT NULL,
  CONSTRAINT "ReactionTypePost_pk" PRIMARY KEY ("ReactionId")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "ReactionComment" (
  "ReactionId" uuid NOT NULL,
  "UserId" uuid NOT NULL,
  "CommentId" uuid NOT NULL,
  CONSTRAINT "ReactionComment_pk" PRIMARY KEY ("ReactionId","UserId")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "ReactionPost" (
  "ReactionId" uuid NOT NULL,
  "UserId" uuid NOT NULL,
  "PostId" uuid NOT NULL,
  CONSTRAINT "ReactionPost_pk" PRIMARY KEY ("ReactionId","UserId")
) WITH (
  OIDS=FALSE
);

ALTER TABLE "ReactionComment" ADD CONSTRAINT "ReactionToComment_fk0" FOREIGN KEY ("CommentId") REFERENCES "Comment"("Id");
ALTER TABLE "ReactionPost" ADD CONSTRAINT "ReactionToPost_fk0" FOREIGN KEY ("PostId") REFERENCES "Post"("Id");


ALTER TABLE "ReactionComment" ADD CONSTRAINT "ReactionComment_fk0" FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypeComment"("ReactionId");
ALTER TABLE "ReactionComment" ADD CONSTRAINT "ReactionComment_fk1" FOREIGN KEY ("UserId") REFERENCES "User"("Id");

ALTER TABLE "ReactionPost" ADD CONSTRAINT "ReactionPost_fk0" FOREIGN KEY ("ReactionId") REFERENCES "ReactionTypePost"("ReactionId");
ALTER TABLE "ReactionPost" ADD CONSTRAINT "ReactionPost_fk1" FOREIGN KEY ("UserId") REFERENCES "User"("Id");