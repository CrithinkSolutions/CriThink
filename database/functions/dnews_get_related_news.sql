CREATE OR REPLACE FUNCTION public.dnews_get_related_news(
    db_news_keywords text,
    user_news_keywords text []
) RETURNS integer LANGUAGE plpgsql AS $$ DECLARE n_common_keywords INTEGER;

BEGIN
SELECT
    array_length(
        ARRAY(
            SELECT
                UNNEST(string_to_array(db_news_keywords, ','))
            INTERSECT
            SELECT
                UNNEST(user_news_keywords)
        ),
        1
    ) INTO n_common_keywords;

IF n_common_keywords > 0 THEN RETURN n_common_keywords;

ELSE RETURN -1;

END if;

END;

$$