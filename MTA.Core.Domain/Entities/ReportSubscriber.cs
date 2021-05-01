using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class ReportSubscriber : EntityModel
    {
        [Column("reportId", true)] public string ReportId { get; protected set; }
        [Column("userId", true)] public int UserId { get; protected set; }

        public Report Report { get; protected set; }

        public static ReportSubscriber Create(string reportId, int userId) => new ReportSubscriber
        {
            ReportId = reportId,
            UserId = userId
        };
    }
}