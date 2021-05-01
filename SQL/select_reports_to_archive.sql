set @closedStatus = 4;

select * from reports
where date_add(dateClosed, interval 7 day) < now()
and status = @closedStatus;