BEGIN;

CREATE TEMPORARY TABLE tbl_Result AS
SELECT id, time_received, message
FROM tbl_MessageQueue
WHERE alive
ORDER BY time_received
LIMIT 1;

UPDATE tbl_MessageQueue
SET alive = FALSE
WHERE id IN (SELECT id FROM tbl_Result);

SELECT time_received, message
FROM tbl_Result;

DROP TABLE tbl_Result;

COMMIT;