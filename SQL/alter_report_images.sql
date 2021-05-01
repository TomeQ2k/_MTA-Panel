alter table report_images
add column userId int not null;

alter table report_images
add column size int not null;

alter table report_images
add foreign key (userId) references konta(id) on delete no action;