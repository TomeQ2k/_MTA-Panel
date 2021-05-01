set @orderType = 0;

select don.id, name, cost, date, account, k.id, k.username from don_purchases as don
join konta as k on don.account = k.id
where account = 71
order by
      case when @orderType = 0 then don.date end asc,
      case when @orderType = 0 then don.date end desc;

set @orderType = 0;

select don.id, name, cost, date, account, k.id, k.username from don_purchases as don
join konta as k on don.account = k.id
where account in (5771, 5774)
order by
      case when @orderType = 0 then don.date end asc,
      case when @orderType = 0 then don.date end desc;


SELECT p.id,p.name,p.cost,p.date,p.account,k.id,k.username FROM don_purchases AS p
LEFT JOIN konta as k ON p.account = k.id
WHERE IF( 'erk' IS NOT NULL , k.username LIKE '%erk%', k.username)
ORDER BY p.date ASC ;
