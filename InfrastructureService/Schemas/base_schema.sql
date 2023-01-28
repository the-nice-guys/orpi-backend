create table if not exists infrastructures
(
    id          serial
        constraint infrastructures_pk
            primary key,
    name        text,
    description text,
    icon        text
);

alter table infrastructures
    owner to postgres;

create unique index if not exists infrastructures_id_uindex
    on infrastructures (id);

create table if not exists user_infrastructures
(
    user_id           integer not null,
    infrastructure_id integer not null
        references infrastructures
            on delete cascade,
    primary key (user_id, infrastructure_id)
);

alter table user_infrastructures
    owner to postgres;

create table if not exists hosts
(
    id          serial
        constraint hosts_pk
            primary key,
    name        text,
    description text,
    icon        text,
    status      integer,
    ip          text
);

alter table hosts
    owner to postgres;

create unique index if not exists hosts_id_uindex
    on hosts (id);

create table if not exists infrastructure_host
(
    infrastructure_id integer not null
        references infrastructures
            on delete cascade,
    host_id           integer not null
        references hosts
            on delete cascade,
    primary key (infrastructure_id, host_id)
);

alter table infrastructure_host
    owner to postgres;

create table if not exists services
(
    id            serial
        constraint services_pk
            primary key,
    name          text,
    description   text,
    "lastUpdated" timestamp,
    dependencies  text[]
);

alter table services
    owner to postgres;

create unique index if not exists services_id_uindex
    on services (id);

create table if not exists host_service
(
    host_id    integer not null
        references hosts
            on delete cascade,
    service_id integer not null
        references services
            on delete cascade,
    primary key (host_id, service_id)
);

alter table host_service
    owner to postgres;

create table if not exists options
(
    id    bigserial
        constraint options_pk
            primary key,
    type  text,
    value text
);

alter table options
    owner to postgres;

create unique index if not exists options_id_uindex
    on options (id);

create table if not exists service_options
(
    service_id integer not null
        references services
            on delete cascade,
    option_id  integer not null
        references options
            on delete cascade,
    primary key (service_id, option_id)
);

alter table service_options
    owner to postgres;