CREATE TABLE articles (
  id VARCHAR(255) PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  content VARCHAR(2000) NOT NULL,
  dateCreated DATETIME DEFAULT NOW(),
  imageUrl VARCHAR(255),
  category INT DEFAULT 0
);

CREATE TABLE changelogs (
  id VARCHAR(255) PRIMARY KEY,
  title VARCHAR(100) NOT NULL,
  content VARCHAR(5000) NOT NUll,
  dateCreated DATETIME DEFAULT NOW(),
  imageUrl VARCHAR(255)
);

CREATE TABLE article_images(
    id VARCHAR(255) NOT NULL PRIMARY KEY,
    path varchar(255) NOT NULL,
    dateCreated DATETIME  NOT NULL,
    articleId VARCHAR(255) NOT NULL,
    FOREIGN KEY (articleId) REFERENCES articles(id) ON DELETE CASCADE
);

CREATE TABLE changelog_images(
    id VARCHAR(255) NOT NULL PRIMARY KEY,
    path varchar(255) NOT NULL,
    dateCreated DATETIME  NOT NULL,
    changelogId VARCHAR(255) NOT NULL,
    FOREIGN KEY (changelogId) REFERENCES changelogs(id) ON DELETE CASCADE
);
