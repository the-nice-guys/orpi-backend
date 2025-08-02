CREATE TABLE IF NOT EXISTS updates 
(
    id SERIAL,
    request_id VARCHAR(64),
    server_ip_address VARCHAR(32),
    update_type INT NOT NULL,
    payload TEXT NOT NULL,
    is_deleted BOOLEAN NOT NULL,
    created_on DATE NOT NULL,
    PRIMARY KEY(request_id)
);

CREATE INDEX new_update_index
ON updates (server_ip_address)
WHERE NOT is_deleted;
