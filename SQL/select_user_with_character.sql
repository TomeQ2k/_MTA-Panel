select * from konta as k
    left join postacie as p on p.account = k.id
    left join factions f on p.faction_id = f.id
    left join pojazdy as pj on pj.owner = p.id
    left join pojazdy_shop as ps on ps.vehmtamodel = pj.model
    left join nieruchomosci as n on n.owner = p.id
    left join bans as b on k.id = b.account
    where k.id = 2;

# select vehmodel, vehbrand, vehyear from pojazdy_shop;
# select name, id, owner from nieruchomosci;
# select id from postacie;
# select * from konta;
#
# select * from postacie as p left join factions as f on f.id = p.faction_id;
# select * from konta as k left join postacie as p on p.account = k.id group by k.id ;
#
# SELECT  model, plate, lastUsed, protected_until, pojazdy_shop.vehbrand, pojazdy_shop.vehmodel, pojazdy_shop.vehyear FROM pojazdy JOIN pojazdy_shop ON pojazdy.model=pojazdy_shop.vehmtamodel WHERE pojazdy.owner=2