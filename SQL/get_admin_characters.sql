set @minLastLogin = '2020-12-24 12:00:00';
set @maxLastLogin = '2021-12-24 12:00:00';
set @minCreationDate = '2020-01-12 12:00:00';
set @maxCreationDate = '2021-01-12 12:00:00';
set @charactername = 'a';
set @gender = 1;
set @active = 0;
set @cked = 0;
set @sortType = 13;

select p.id, p.charactername, p.money, p.bankmoney, p.gender, p.lastarea, p.hoursplayed, p.lastlogin, p.creationdate, p.active, p.account, p.cked,
       p.money + p.bankmoney as TotalMoney
from postacie as p
where p.lastlogin between @minLastLogin and @maxLastLogin
and p.creationdate between @minCreationDate and @maxCreationDate
and if(@charactername is not null, p.charactername like '%a%', p.charactername)
and case
    when @gender = 0 then p.gender in (0, 1)
    when @gender = 1 then p.gender = 0
    when @gender = 2 then p.gender = 1
end
and case
    when @active = 0 then p.active in (0, 1)
    when @active = 1 then p.active = 1
    when @active = 2 then p.active = 0
end
and case
    when @cked = 0 then p.cked in (0, 1)
    when @cked = 1 then p.cked = 0
    when @cked = 2 then p.cked = 1
end
order by
    case when @sortType = 0 then p.id end,
    case when @sortType = 1 then p.id end desc,
    case when @sortType = 2 then p.charactername end,
    case when @sortType = 3 then p.charactername end desc,
    case when @sortType = 4 then TotalMoney end,
    case when @sortType = 5 then TotalMoney end desc,
    case when @sortType = 6 then p.hoursplayed end,
    case when @sortType = 7 then p.hoursplayed end desc,
    case when @sortType = 8 then p.lastlogin end,
    case when @sortType = 9 then p.lastlogin end desc,
    case when @sortType = 10 then p.creationdate end,
    case when @sortType = 11 then p.creationdate end desc,
    case when @sortType = 12 then p.active end,
    case when @sortType = 13 then p.active end desc;