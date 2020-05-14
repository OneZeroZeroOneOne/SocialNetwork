CREATE or replace FUNCTION UpdateTop(integer, uuid, integer) RETURNS real AS $$
DECLARE
    postId ALIAS FOR $1;
    boardId ALIAS FOR $2;
    minusScore ALIAS FOR $3;
BEGIN
		update "public"."PostTop" set "Score" = "Score" - minusScore where "PostId" != postId and "BoardId" = boardId;
    RETURN 1;
END;
$$ LANGUAGE plpgsql;