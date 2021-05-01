select p.id as CharacterId, p.money, p.bankmoney, n.id as EstateId, poj.id as VehicleId, i.`index` as ItemId
from postacie as p
         join nieruchomosci as n on n.owner = p.id
         join pojazdy as poj on poj.owner = p.id
         join items as i on i.owner = p.id
where p.id = 2
  and i.type = 1;

select *
from items
where owner = 2;

select *
from nieruchomosci
where owner = 2;

select *
from pojazdy
where owner = 2;

select * from nieruchomosci where id in (812, 813);

update items
set owner = 333
where owner = 2;
update nieruchomosci
set owner = 333
where owner = 2;
update pojazdy
set owner = 333
where owner = 2;
