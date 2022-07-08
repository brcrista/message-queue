INSERT INTO tbl_MessageQueue
(time_received, message, alive)
VALUES (datetime('now'), 'hello', TRUE);