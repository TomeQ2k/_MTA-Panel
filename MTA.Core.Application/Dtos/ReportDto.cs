using System;
using System.Collections.Generic;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class ReportDto
    {
        public string Id { get; set; }
        public string CreatorId { get; set; }
        public string AssigneeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? EventDate { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public ReportStatusType Status { get; set; }
        public bool IsPrivate { get; set; }
        public ReportCategoryType CategoryType { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? DateClosed { get; set; }
        public string UserName { get; set; }
        public string AdminName { get; set; }

        public PenaltyReportDto PenaltyReport { get; set; }
        public UserReportDto UserReport { get; set; }
        public BugReportDto BugReport { get; set; }

        public ICollection<ReportCommentDto> ReportComments { get; set; }
        public ICollection<ReportSubscriberDto> ReportSubscribers { get; set; }
        public ICollection<ReportImageDto> ReportImages { get; set; }
    }
}