CREATE or replace FUNCTION IsPostOrComment(integer) RETURNS real AS $$
DECLARE
    cId ALIAS FOR $1;
BEGIN
    IF EXISTS (select "Id" from "public"."Comment" c where c."Id" = cId) 
    THEN
        return 1;
    ELSIF EXISTS (select "Id" from "public"."Post" c where c."Id" = cId) 
    THEN
        return 0;
    ELSE
        return -1;
    END IF;
END;
$$ LANGUAGE plpgsql;