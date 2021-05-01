set @username = 'erkker';
set @stateStatusType = -1;
set @sortType = 0;

select * from orders as o
left join order_files as f on f.orderId = o.id
left join konta as k on o.userId = k.id
where if(@username is not null and k.username is not null, k.username like '%erkker%', k.username)
and case
    when @stateStatusType = -1 then o.approvalState in (0,1,2)
    when @stateStatusType = 0 then o.approvalState = 0
    when @stateStatusType = 1 then o.approvalState = 1
    when @stateStatusType = 2 then o.approvalState = 2
end order by
    case when @sortType = 0 then o.dateCreated end,
    case when @sortType = 1 then o.dateCreated end desc;

show columns in orders;

insert into orders(id, operation, cost, userId, characterId, estateId, approvalState, adminNote) values('ABC12345', 1, 25, 5771, null, null, 0, 'Siema 1!');
insert into orders(id, operation, cost, userId, characterId, estateId, approvalState, adminNote) values('BBC12345', 2, 20, 5771, null, null, 1, 'Siema 2!');
insert into orders(id, operation, cost, userId, characterId, estateId, approvalState, adminNote) values('CBC12345', 1, 1, 5769, null, null, 1, 'Siema 3!');
insert into orders(id, operation, cost, userId, characterId, estateId, approvalState, adminNote) values('DBC12345', 0, 0, 5771, null, null, 2, 'Siema 4!');
insert into orders(id, operation, cost, userId, characterId, estateId, approvalState, adminNote) values('EBC12345', 0, 100, 5769, null, null, 0, 'Siema 5!');

insert into order_files(id, url, path, dateCreated, orderId) values('FILE1', 'URL', 'PATH', now(), 'ABC12345');
insert into order_files(id, url, path, dateCreated, orderId) values('FILE2', 'URL', 'PATH', now(), 'BBC12345');
insert into order_files(id, url, path, dateCreated, orderId) values('FILE3', 'URL', 'PATH', now(), 'CBC12345');
insert into order_files(id, url, path, dateCreated, orderId) values('FILE4', 'URL', 'PATH', now(), 'DBC12345');