set @userId = 2;

select * from postacie as p
left join nieruchomosci as n on n.owner = p.id
left join pojazdy as poj on poj.owner = p.id
where p.account = @userId;