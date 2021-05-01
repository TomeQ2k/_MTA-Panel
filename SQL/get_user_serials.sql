set @userId = 42;

select id, serial, last_login_ip, last_login_date, userid from serial_whitelist
where userid = @userId and last_login_date is not null and status = 1
order by last_login_date desc;

