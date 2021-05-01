#Check columns in table
show columns from orders;

create index idx_email
on konta (email);

create index idx_username
on konta (username);

create index idx_account
on postacie (account);

create index idx_owner
on nieruchomosci (owner);

create index idx_owner
on pojazdy (owner);

create index idx_owner
on items (owner);

create index idx_userid
on serial_whitelist (userid);

create index idx_userId
on notifications (userid);

create index idx_creatorId
on reports (creatorId);

create index idx_assigneeId
on reports (assigneeId);

create index idx_reportId
on penalty_reports (reportId);

create index idx_reportId
on user_reports (reportId);

create index idx_reportId
on bug_reports (reportId);

create index idx_userId
on report_comments (userId);

create index idx_userId
on report_subscribers (userId);

create index idx_reportId
on report_subscribers (reportId);

create index idx_user
on adminhistory (user);

create index idx_user_char
on adminhistory (user_char);

create index idx_admin
on adminhistory (admin);

create index idx_account
on don_purchases (account);

create index idx_transactionId
on don_transactions (transaction_id);

create index idx_userId
on orders (userId);

create index idx_characterId
on orders (characterId);

create index idx_estateId
on orders (estateId);