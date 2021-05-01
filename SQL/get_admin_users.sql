set @minRegisterDate = '2020-08-21 13:50:00';
set @maxRegisterDate = '2020-08-25 12:00:00';
set @minLastLogin = '2020-08-21 13:50:00';
set @maxLastLogin = '2020-12-25 12:00:00';
set @activated = 1;
set @appState = 0;
set @banStatus = 2;
set @ip = '127';
set @serial = 'E';
set @sortType = 1;

select k.id, k.username, k.email, k.mtaserial, k.activated, k.lastlogin, k.registerdate, k.ip, k.appstate, b.id as 'BanId' from konta as k
left join bans as b on b.account = k.id
where k.registerdate between @minRegisterDate and @maxRegisterDate
and k.lastlogin between @minLastLogin and @maxLastLogin
and case
    when @activated = 0 then k.activated in (0, 1)
    when @activated = 1 then k.activated = 1
    when @activated = 2 then k.activated = 0
end
and case
    when @appState = 0 then k.appstate in (0, 1, 2, 3)
    when @appState = 1 then k.appstate = 1
    when @appState = 2 then k.appstate = 2
    when @appState = 3 then k.appstate = 3
    when @appState = 4 then k.appstate = 0
end
and case
    when @banStatus = 0 then b.id or b.id is null
    when @banStatus = 1 then b.id
    when @banStatus = 2 then b.id is null
end
and if(@ip is not null, k.ip like '%127%', k.ip)
and if(@serial is not null, k.mtaserial like '%E%', k.mtaserial)
order by
    case when @sortType = 0 then k.id end,
    case when @sortType = 1 then k.id end desc,
    case when @sortType = 2 then k.registerdate end,
    case when @sortType = 3 then k.registerdate end desc,
    case when @sortType = 4 then k.lastlogin end,
    case when @sortType = 5 then k.lastlogin end desc;