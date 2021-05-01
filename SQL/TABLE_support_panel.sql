create table reports (
  id varchar(255) not null,
  creatorId int,
  assigneeId int,
  dateCreated datetime default now(),
  dateUpdated datetime default now(),
  eventDate datetime,
  subject varchar(1000),
  content varchar(5000) not null,
  status tinyint not null default 0,
  isPrivate bool default false,
  categoryType tinyint not null default 0,
  isArchived bool default false,
  dateClosed datetime default null,

  primary key (id),
  foreign key (creatorId) references konta(id) on delete set null,
  foreign key (assigneeId) references konta(id) on delete set null
);

create table penalty_reports (
    id varchar(255) not null,
    reportId varchar(255) not null,
    banId int,
    penaltyId int,

    primary key (id),
    foreign key (reportId) references reports(id) on delete cascade,
    foreign key (banId) references bans(id) on delete set null,
    foreign key (penaltyId) references adminhistory(id) on delete set null
);

create table user_reports (
    id varchar(255) not null,
    reportId varchar(255) not null,
    userId int,
    witnessId int,

    primary key (id),
    foreign key (reportId) references reports(id) on delete cascade,
    foreign key (userId) references konta(id) on delete set null,
    foreign key (witnessId) references konta(id) on delete set null
);

create table bug_reports (
    id varchar(255) not null,
    reportId varchar(255) not null,
    bugType tinyint not null default 0,
    additionalInfo varchar(500),

    primary key (id),
    foreign key (reportId) references reports(id) on delete cascade
);

create table report_subscribers (
    reportId varchar(255) not null,
    userId int not null,

    primary key (reportId, userId),
    foreign key (reportId) references reports(id) on delete cascade,
    foreign key (userId) references konta(id) on delete cascade
);

create table report_comments (
    id varchar(255) not null,
    reportId varchar(255) not null,
    userId int not null,
    content varchar(3000) not null,
    dateCreated datetime default now(),
    isPrivate bool default false,

    primary key (id),
    foreign key (reportId) references reports(id) on delete cascade,
    foreign key (userId) references konta(id) on delete no action
);

create table report_images (
    id varchar(255) not null,
    url varchar(255) not null,
    path varchar(255) not null,
    dateCreated datetime default now(),
    reportId varchar(255) not null,
    userId int not null,
    size int not null,

    primary key (id),
    foreign key (reportId) references reports(id) on delete no action,
    foreign key (userId) references konta(id) on delete no action
);
