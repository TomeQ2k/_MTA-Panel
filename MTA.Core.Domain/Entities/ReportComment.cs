using System;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class ReportComment : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("reportId")] public string ReportId { get; protected set; }
        [Column("userId")] public int UserId { get; protected set; }
        [Column("content")] public string Content { get; protected set; }
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("isPrivate")] public int IsPrivate { get; protected set; }

        public Report Report { get; protected set; }

        public bool Private => IsPrivate == 1;

        public void SetReportId(string reportId) => ReportId = reportId;

        public void SetUserId(int userId) => UserId = userId;

        public void SetContent(string content) => Content = content;

        public void SetPrivacy(bool isPrivate) => IsPrivate = isPrivate ? 1 : 0;
    }
}