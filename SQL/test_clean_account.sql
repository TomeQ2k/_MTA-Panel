select p.id, n.id, poj.id, i.`index`, k.id from postacie p
join nieruchomosci n on p.id = n.owner
join pojazdy poj on poj.owner = p.id
join items i on i.owner = p.id
join konta k on k.id = p.account
where p.id in (4987,4988);

select id from konta where id = 5771;