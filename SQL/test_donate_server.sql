select id, username, referrer, hours, credits from konta
where referrer = 5771 or id = 5771;

select * from notifications order by date desc;

update konta set hours = 35 where id = 5769;

update konta set credits = 0 where id = 5769 or id = 5771;

update don_transactions set validated = 1;

select * from don_transactions;
select * from tokens;

select * from reports order by dateCreated desc
limit 1;