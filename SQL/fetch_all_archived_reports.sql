set @category = -1;
set @status = -2;
set @adminId = null;
set @sortType = 1;

select r.id, creatorId, assigneeId, dateCreated, dateUpdated, subject, status, isPrivate, categoryType, k.username as UserName, kk.username as AdminName from reports as r
left join konta as k on r.assigneeId = k.id
join konta as kk on kk.id = r.creatorId
where r.status = -1
and if(@adminId is not null, r.assigneeId = @adminId, r.assigneeId )
and case
    when @category = -1 then r.categoryType between 0 and 6
    when @category = 0 then r.categoryType = 0
    when @category = 1 then r.categoryType = 1
    when @category = 2 then r.categoryType = 2
    when @category = 3 then r.categoryType = 3
    when @category = 4 then r.categoryType = 4
    when @category = 5 then r.categoryType = 5
    when @category = 6 then r.categoryType = 6
end
order by
    case when @sortType = 0 then r.dateUpdated end,
    case when @sortType = 1 then r.dateUpdated end desc;
