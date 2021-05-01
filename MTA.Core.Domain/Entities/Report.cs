using System;
using System.Collections.Generic;
using System.Linq;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Report : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("creatorId")] public int CreatorId { get; protected set; }
        [Column("assigneeId")] public int? AssigneeId { get; protected set; }
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("dateUpdated")] public DateTime DateUpdated { get; protected set; } = DateTime.Now;
        [Column("eventDate")] public DateTime? EventDate { get; protected set; }
        [Column("subject")] public string Subject { get; protected set; }
        [Column("content")] public string Content { get; protected set; }
        [Column("status")] public int Status { get; protected set; } = (int) ReportStatusType.Awaiting;
        [Column("isPrivate")] public int IsPrivate { get; protected set; }
        [Column("categoryType")] public int CategoryType { get; protected set; }
        [Column("isArchived")] public int IsArchived { get; protected set; }
        [Column("dateClosed")] public DateTime? DateClosed { get; protected set; }

        [Column("UserName", customProperty: true)]
        public string UserName { get; protected set; }

        [Column("AdminName", customProperty: true)]
        public string AdminName { get; protected set; }

        public PenaltyReport PenaltyReport { get; protected set; }
        public UserReport UserReport { get; protected set; }
        public BugReport BugReport { get; protected set; }

        public ICollection<ReportComment> ReportComments { get; protected set; } =
            new HashSet<ReportComment>();

        public ICollection<ReportSubscriber> ReportSubscribers { get; protected set; } =
            new HashSet<ReportSubscriber>();

        public ICollection<ReportImage> ReportImages { get; protected set; } = new HashSet<ReportImage>();

        public bool Private => IsPrivate == 1;
        public bool Archived => IsArchived == 1;

        public void CreatedBy(int userId) => CreatorId = userId;
        public void SetSubject(string subject) => Subject = subject;
        public void SetContent(string content) => Content = content;
        public void SetEventDate(DateTime? eventDate) => EventDate = eventDate;
        public void SetCategoryType(ReportCategoryType categoryType) => CategoryType = (int) categoryType;
        public void SetPrivacy(bool isPrivate) => IsPrivate = isPrivate ? 1 : 0;
        public void TogglePrivacy() => IsPrivate = IsPrivate == 1 ? 0 : 1;
        public void AssigneTo(int? userId) => AssigneeId = userId;
        public void ChangeStatus(ReportStatusType status) => Status = (int) status;

        public void Close()
        {
            Status = (int) ReportStatusType.Closed;
            DateClosed = DateTime.Now;
        }

        public void Archive()
        {
            Status = (int) ReportStatusType.Archived;
            IsArchived = 1;
        }

        public void UpdateDate() => DateUpdated = DateTime.Now;

        public void SetBugReport(BugReport bugReport) => BugReport = bugReport;
        public void SetPenaltyReport(PenaltyReport penaltyReport) => PenaltyReport = penaltyReport;
        public void SetUserReport(UserReport userReport) => UserReport = userReport;

        public void SetImages(IEnumerable<ReportImage> reportImages) => ReportImages = reportImages.ToList();

        public bool HasArchived => DateClosed.HasValue
            ? DateClosed.Value.AddDays(Constants.ReportArchivizationTimeInDays) < DateTime.Now
            : false;

        public bool IsStatus(ReportStatusType statusType) => Status == (int) statusType;
    }
}