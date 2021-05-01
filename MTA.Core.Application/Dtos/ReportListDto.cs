using System;
using System.Collections.Generic;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class ReportListDto
    {
        public string Id { get; set; }
        public string CreatorId { get; set; }
        public string AssigneeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? EventDate { get; set; }
        public string Subject { get; set; }
        public ReportStatusType Status { get; set; }
        public bool IsPrivate { get; set; }
        public ReportCategoryType CategoryType { get; set; }

        public string UserName { get; set; }
        public string AdminName { get; set; }
    }
}