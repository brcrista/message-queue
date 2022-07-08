CREATE TABLE IF NOT EXISTS tbl_MessageQueue (
    id INTEGER PRIMARY KEY,
    time_received TIMESTAMP,
    message TEXT,
    alive BOOLEAN
);