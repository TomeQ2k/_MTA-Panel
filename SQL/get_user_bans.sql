set @userId = 5771;

select ah.id, ah.date, ah.reason, ah.user as AccountId, ah.admin as AdminId, ah.duration from adminhistory as ah
where ah.user = @userId and ah.action = 2;

select b.id, b.date, b.reason, b.admin as AdminId, b.account as UserId from bans as b
where b.account = @userId;