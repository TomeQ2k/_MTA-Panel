using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class UserReport : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("reportId")] public string ReportId { get; protected set; }
        [Column("userId")] public int? UserId { get; protected set; }
        [Column("witnessId")] public int? WitnessId { get; protected set; }

        public Report Report { get; protected set; }

        public static UserReport Create(string reportId, int? userId, int? witnessId) => new UserReport
        {
            ReportId = reportId,
            UserId = userId,
            WitnessId = witnessId
        };

        public void SetReport(Report report) => Report = report;
    }
}