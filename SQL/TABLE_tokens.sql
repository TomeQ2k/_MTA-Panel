USE tlife_project;

DROP TABLE tokens;
CREATE TABLE tokens (id VARCHAR(255) NOT NULL, PRIMARY KEY(id),
    code VARCHAR(255) NOT NULL,
    dateCreated DATE NOT NULL,
    tokenType INT NOT NULL,
    userId INT NOT NULL, FOREIGN KEY(userId) REFERENCES konta(id) ON DELETE CASCADE );
SELECT * FROM tokens;