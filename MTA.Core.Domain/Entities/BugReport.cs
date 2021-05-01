using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class BugReport : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("reportId")] public string ReportId { get; protected set; }
        [Column("bugType")] public int BugType { get; protected set; } = (int) ReportBugType.MTA;
        [Column("additionalInfo")] public string AdditionalInfo { get; protected set; }

        public Report Report { get; protected set; }

        public static BugReport Create(string reportId, ReportBugType bugType, string additionalInfo) => new BugReport
        {
            ReportId = reportId,
            BugType = (int) bugType,
            AdditionalInfo = additionalInfo
        };

        public void SetReport(Report report) => Report = report;
    }
}