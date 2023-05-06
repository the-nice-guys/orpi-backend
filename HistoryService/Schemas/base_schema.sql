create table if not exists history
(
    id          serial
        constraint history_pk
            primary key,
    infrastructure_id integer,
    timestamp        timestamp,
    message text
);

alter table history
    owner to postgres;

create unique index if not exists history_id_uindex
    on history (id);

create index if not exists history_infrastructure_id_uindex
    on history (infrastructure_id);