CREATE OR REPLACE FUNCTION public.createcomment(integer, text, uuid)
 RETURNS TABLE("Id" int8,
	"UserId" uuid,
	"PostId" int8,
	"Text" text,
	"Date" timestamp,
	"IsArchived" bool,
	"SeqId" int8)
 LANGUAGE plpgsql
AS $function$
DECLARE
    cId ALIAS FOR $1;
    cText ALIAS FOR $2;
    cUserId ALIAS FOR $3;
BEGIN
		return query INSERT INTO public."Comment"
			("UserId", 
			"PostId", 
			"Text", 
			"Date", 
			"IsArchived", 
			"SeqId")
		select
			cUserId, 
			cId, 
			cText, 
			current_timestamp, 
			false,
			count(c."Id")
		from
			public."Comment" c
		where c."PostId" = cId
		returning *;
END;
$function$
;