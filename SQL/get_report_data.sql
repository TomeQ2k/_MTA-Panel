select k.id, p.id, k.username, p.charactername as CharacterName from postacie as p
join konta as k on k.id = p.account
group by k.id, p.id, k.username, p.charactername
having p.charactername like '%rado%';

set @action = 1;
set @userId = 5771;

select ah.id, ah.date, ah.action, ah.duration, ah.reason, ku.username as UserName, ka.username as AdminName, p.charactername as CharacterName
from adminhistory as ah
left join konta as ku on ku.id = ah.user
left join konta as ka on ka.id = ah.admin
left join postacie as p on p.id = ah.user_char
where ah.action = @action and ah.user = @userId;