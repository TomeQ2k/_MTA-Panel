set @userId = 34;

select exists(
    select * from serial_whitelist
    where serial = 'C07DE92E88735B0EAE89917B11885AF2' and userid = @userId
) as SerialExists;

select id, serial, last_login_ip, last_login_date, userid from serial_whitelist
where userid = @userId and last_login_date is not null and status = 1
order by last_login_date desc;

delete from bans where serial = '050AE16007BDBE4243E5BD952BEF97F3' or ip = '195.191.163.2';

select * from bans
order by date desc;

