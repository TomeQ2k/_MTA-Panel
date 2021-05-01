select * from reports as r
left join report_comments as rc on r.id = rc.reportId
left join report_subscribers as rs on r.id = rs.reportId
left join report_images ri on r.id = ri.reportId
left join bug_reports br on r.id = br.reportId
left join penalty_reports pr on r.id = pr.reportId
left join user_reports ur on r.id = ur.reportId
where r.id = 'BDF11BEA1F1D41FA';
