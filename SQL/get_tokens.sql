USE tlife_project;

SELECT * FROM tokens JOIN konta as k ON tokens.userId = k.id WHERE k.id = 5769;
