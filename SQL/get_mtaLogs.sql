set @minTimeAgo = '2021-02-17 09:00:00';
set @maxTimeAgo = '2021-02-17 10:20:00';
set @content = '';
set @contentFilter = 0;
set @sourceAffected = '';
set @sourceAffectedFilter = 1;
set @dateSortType = 0;

#GetMtaLogs
SELECT * FROM mta_logi
WHERE action IN ('31','11','7','40','4')
AND time BETWEEN '2020-10-08 12:25:25' AND '2021-04-09 11:25:25'
AND CASE
    WHEN @contentFilter = 0 THEN content LIKE '%%'
    WHEN @contentFilter = 1 THEN content = @content
    WHEN @contentFilter = 2 THEN content LIKE CONCAT('%', @content,'%')
END
AND CASE
      WHEN @sourceAffectedFilter = 0 THEN source LIKE CONCAT('%','ch4198','%') OR source like CONCAT('%','ch2748','%') OR affected
        LIKE CONCAT('%','ch4198','%') OR affected like CONCAT('%','ch2748','%')
      WHEN @sourceAffectedFilter = 1 THEN source LIKE CONCAT('%','ch4198','%') OR source like CONCAT('%','ch2748','%')
      WHEN @sourceAffectedFilter = 2 THEN affected LIKE CONCAT('%','ch4198','%') OR affected like CONCAT('%','ch2748','%')
END
ORDER BY CASE WHEN 1 = 0 THEN time END , CASE WHEN 1 = 1 THEN time END DESC;

#GetPhoneSms
select * from phone_sms as p
where p.date between @minTimeAgo and @maxTimeAgo
and case
    when @contentFilter = 0 then p.content like '%%'
    when @contentFilter = 1 then p.content = @content
    when @contentFilter = 2 then p.content like CONCAT('%', @content ,'%')
end
  and case
      when  @sourceAffectedFilter = 0 then p.from like concat('%', @sourceAffected ,'%') or `to` like concat('%', @sourceAffected ,'%')
      when  @sourceAffectedFilter = 1 then p.from like concat('%', @sourceAffected ,'%')
      when  @sourceAffectedFilter = 2 then p.to like concat('%', @sourceAffected ,'%')
end
order by
    case when @dateSortType = 0 then p.date end,
    case when @dateSortType = 1 then p.date end desc;