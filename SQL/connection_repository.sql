set @userId = 5771;
set @hubName = 'NOTIFIER';

#GetUserHubConnections
select * from connections
where userId = @userId and hubName = @hubName;

#FindByConnectionId
select * from connections
where userId = @userId and hubName = @hubName
limit 1;
