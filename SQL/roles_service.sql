USE tlife_project;

#AdmitRole (userId = 6611, supporter = 2)
UPDATE konta SET supporter = 2 WHERE id = 6611;

#RevokeRole (userId = 6611)
UPDATE konta SET supporter = 0 WHERE id = 6611;

#IsPermitted (userId = 6611, admin = 3, mapper = 2)
SELECT EXISTS(SELECT * FROM konta WHERE id = 6611 AND admin = 3) as IsPermitted; #true
SELECT EXISTS(SELECT * FROM konta WHERE id = 6611 AND mapper = 2) as IsPermitted; #false

#SELECT id with roles FROM konta
SELECT id,admin,supporter,vct,mapper,scripter FROM konta;