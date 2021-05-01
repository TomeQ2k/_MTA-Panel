using System.Collections.Generic;
using System.Linq;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.ServicesUtils
{
    public static class ReportManagerUtils
    {
        public static IEnumerable<int> GenerateReportMembersGroup(Report report, int currentUserId)
        {
            var membersGroup = new List<int>(report.ReportSubscribers.Select(rs => rs.UserId));

            membersGroup.Add(report.CreatorId);

            if (report.AssigneeId != null)
                membersGroup.Add(report.AssigneeId.Value);

            return membersGroup.Except(new List<int> {currentUserId});
        }
    }
}