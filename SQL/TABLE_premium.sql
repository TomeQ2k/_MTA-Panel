create table orders
(
    id            varchar(255) not null,
    operation     int          not null default 0,
    cost          int          not null default 0,
    userId        int null,
    characterId   int null,
    estateId      int null,
    approvalState int          not null default 0,
    adminNote     varchar(1000) null,
    dateCreated   datetime              default now(),
    dateReviewed  datetime null,

    primary key (id),
    foreign key (userId) references konta (id) on delete set null,
    foreign key (characterId) references postacie (id) on delete set null,
    foreign key (estateId) references nieruchomosci (id) on delete set null
);

create table premium_files
(
    id          varchar(255) NOT NULL,
    url         varchar(255) NOT NULL,
    path        varchar(255) NOT NULL,
    dateCreated datetime     NOT NULL,
    orderId     varchar(255) not null,
    userId      int          not null,
    fileType    int          not null default 0,
    inUse       bit          not null defaul 0,
    skinId      int          null,

    primary key (id),
    foreign key (orderId) references orders (id) on delete cascade
);

show
create table orders;
show
create table order_files;
