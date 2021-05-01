using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class PenaltyReport : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("reportId")] public string ReportId { get; protected set; }
        [Column("banId")] public int? BanId { get; protected set; }
        [Column("penaltyId")] public int? PenaltyId { get; protected set; }

        public Report Report { get; protected set; }

        public static PenaltyReport Create(string reportId, int? banId, int? penaltyId) => new PenaltyReport
        {
            ReportId = reportId,
            BanId = banId,
            PenaltyId = penaltyId
        };

        public void SetReport(Report report) => Report = report;
    }
}