select k.id , k.username from konta as k
left join reports as r on k.id = r.assigneeId
where k.admin > 0 or k.supporter > 0;