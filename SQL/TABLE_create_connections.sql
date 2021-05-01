create table connections (
    userId int not null,
    connectionId varchar(255) not null,
    dateEstablished datetime default now(),
    hubName varchar(255) not null,
    primary key (userId, connectionId),
    foreign key (userId) references konta (id)
);