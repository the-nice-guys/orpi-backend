CREATE TABLE IF NOT EXISTS users(
    id SERIAL,
    login TEXT PRIMARY KEY,
    email TEXT NOT NULL,
    name TEXT NOT NULL,
    surname TEXT NOT NULL
);