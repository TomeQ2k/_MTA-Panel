#Update tlife_project files urls and paths in all tables

#Created by: Tomasz Nowok 26.04.2021

#Specify old fragments of path and url which should be replaced [Use \\ to write \]
set @oldPath = 'F:\\.projects\\MTA\\MTA.API\\';
set @oldUrl = 'http://localhost:5000';
#Specify new fragments of path and url which would replace old ones
set @newPath = 'D:\\.proj\\MTAPanel\\';
set @newUrl = 'http://tlife.project.pl';

#Use transaction and COMMIT only IF YOU ARE SURE THAT CHANGES HAVE BEEN MADE SUCCESSFUL!

#Specify pattern to search for old paths and urls (default => old path/url + %)
set @oldPathPattern = 'F:_.projects_MTA_MTA.API%';
set @oldUrlPattern = 'http://localhost:5000%';

select @oldPathPattern, @oldUrlPattern;

###Script

#Find all records to change
select imageUrl
from articles
where imageUrl like @oldUrlPattern;

select url, path
from article_images
where path like @oldPathPattern
   or url like @oldUrlPattern;

select imageUrl
from changelogs
where imageUrl like @oldUrlPattern;

select url, path
from changelog_images
where path like @oldPathPattern
   or url like @oldUrlPattern;

select url, path
from premium_files
where path like @oldPathPattern
   or url like @oldUrlPattern;

select url, path
from report_images
where path like @oldPathPattern
   or url like @oldUrlPattern;

###Start transaction and update all found records
start transaction;

#Updating urls

update articles
set imageUrl = replace(imageUrl, @oldUrl, @newUrl)
where imageUrl like @oldUrlPattern;

update article_images
set url = replace(url, @oldUrl, @newUrl)
where url like @oldUrlPattern;

update changelogs
set imageUrl = replace(imageUrl, @oldUrl, @newUrl)
where imageUrl like @oldUrlPattern;

update changelog_images
set url = replace(url, @oldUrl, @newUrl)
where url like @oldUrlPattern;

update premium_files
set url = replace(url, @oldUrl, @newUrl)
where url like @oldUrlPattern;

update report_images
set url = replace(url, @oldUrl, @newUrl)
where url like @oldUrlPattern;

#Updating paths

update article_images
set path = replace(path, @oldPath, @newPath)
where path like @oldPathPattern;

update changelog_images
set path = replace(path, @oldPath, @newPath)
where path like @oldPathPattern;

update premium_files
set path = replace(path, @oldPath, @newPath)
where path like @oldPathPattern;

update report_images
set path = replace(path, @oldPath, @newPath)
where path like @oldPathPattern;

#Reject all changes made
rollback;
#Accept changes and commit database
commit;