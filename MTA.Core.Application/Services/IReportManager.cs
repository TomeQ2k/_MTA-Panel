using System.Threading.Tasks;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportManager : IReadOnlyReportManager
    {
        Task<bool> ChangeStatus(ReportStatusType reportStatusType, Report report);

        Task<ReportAssignmentResult> AcceptReportAssignment(Report report);
        Task<ReportAssignmentResult> RejectReportAssignment(Report report);

        Task<bool> CloseReport(Report report);

        Task<bool> ArchiveReport(Report report);
        Task<ArchiveReportsResult> ArchiveReports();

        Task<bool> MoveReportAssignment(Report report, int userId);

        Task<bool> AssignAwaitingReports(ReportCategoryType reportCategoryType, bool isPrivate);

        Task<TogglePrivacyReportResult> TogglePrivacyReport(Report report);
    }
}