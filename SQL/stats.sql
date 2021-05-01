USE tlife_project;

#Accounts count
SELECT COUNT(*) as AccountsCount FROM konta;

#Characters count
SELECT COUNT(*) as CharactersCount FROM postacie;

#Estates count
SELECT  COUNT(*) as EstatesCount FROM nieruchomosci;

#Vehicles count
SELECT COUNT(*) as VehiclesCount FROM pojazdy;

#HoursPlayed total
SELECT SUM(hours) as HoursPlayedTotal FROM konta;

#BankMoney total
SELECT SUM(p.bankmoney + f.bankbalance) as BankMoneyTotal FROM postacie as p, factions as f;

SELECT k.id, p.money FROM postacie as p
    JOIN konta as k ON p.account = k.id;

### MERGED STATS SERVER ###
SELECT
       (SELECT COUNT(*) FROM konta) as AccountsCount,
       (SELECT COUNT(*) FROM postacie) as CharactersCount,
       (SELECT COUNT(*) FROM nieruchomosci) as EstatesCount,
       (SELECT COUNT(*) FROM pojazdy) as VehiclesCount,
       (SELECT SUM(hours) FROM konta) as HoursPlayedTotal,
       (SELECT SUM(p.bankmoney + f.bankbalance) FROM postacie as p, factions as f) as BankMoneyTotal;

### TOP10 ###
#MostActivePlayers
SELECT charactername as Nick, hoursplayed as HoursPlayed, lastlogin as LastLogin FROM postacie
    WHERE lastlogin >= DATE_ADD(NOW(), INTERVAL -30 DAY)
    ORDER BY hoursplayed DESC
    LIMIT 10;

#FactionsWithThe
SELECT f.id, f.name, f.bankbalance,
           (SELECT COUNT(*) FROM postacie as p
            WHERE p.faction_id = f.id) as WorkersCount
    FROM factions as f
    ORDER BY f.bankbalance DESC;

#MostActiveAdmins
SELECT id, username, admin, supporter, vct, mapper, scripter, adminreports FROM konta
    WHERE admin + supporter + vct + mapper + scripter > 0
        AND lastlogin >= DATE_ADD(NOW(), INTERVAL -30 DAY)
    ORDER BY adminreports DESC
    LIMIT 10;

SELECT a.id,a.admin FROM adminhistory as a
    JOIN konta as k ON k.id = a.admin;

#RichestPlayers
SELECT
    p.charactername,
    (
         IFNULL(p.money, 0) +
         IFNULL(p.bankmoney, 0) +
         IFNULL((SELECT SUM(n.cost) FROM nieruchomosci AS n WHERE n.owner = p.id), 0) +
         IFNULL((SELECT SUM(pojs.vehprice) FROM pojazdy_shop as pojs WHERE pojs.vehmtamodel IN (SELECT poj.model FROM pojazdy as poj WHERE poj.owner = p.id)), 0)
    ) as TotalMoney
FROM postacie as p
WHERE p.lastlogin >= DATE_ADD(NOW(), INTERVAL -30 DAY)
ORDER BY TotalMoney DESC
LIMIT 10;