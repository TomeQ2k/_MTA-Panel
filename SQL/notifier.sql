set @userId = 5771;
set @notificationId = 'AAABBBCCCCDDDD';

#GetNotifications
select * from notifications
where userId = @userId
order by dateCreated desc;

#GetNotification
select * from notifications
where id = @notificationId
limit 1;
