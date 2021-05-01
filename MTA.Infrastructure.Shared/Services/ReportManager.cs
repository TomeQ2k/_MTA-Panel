using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportManager : IReportManager
    {
        private readonly IDatabase database;

        public ReportManager(IDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<User>> GetReportsAllowedAssignees(ReportCategoryType reportCategoryType,
            bool isPrivate = false)
            => await database.UserRepository.GetUsersWithAssignedReports(reportCategoryType, isPrivate);

        public async Task<bool> ChangeStatus(ReportStatusType reportStatusType, Report report)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            report.ChangeStatus(reportStatusType);
            report.UpdateDate();

            return await database.ReportRepository.Update(report);
        }

        public async Task<ReportAssignmentResult> AcceptReportAssignment(Report report)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            return new ReportAssignmentResult(IsAccepted: true,
                IsSucceeded: await ChangeStatus(ReportStatusType.Assigned, report));
        }

        public async Task<ReportAssignmentResult> RejectReportAssignment(Report report)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            report.AssigneTo(null);

            return new ReportAssignmentResult(IsAccepted: false,
                IsSucceeded: await ChangeStatus(ReportStatusType.Awaiting, report));
        }

        public async Task<bool> CloseReport(Report report)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            if (report.IsStatus(ReportStatusType.Closed))
                throw new NoPermissionsException("Report is already closed");

            report.Close();

            return await database.ReportRepository.Update(report);
        }

        public async Task<bool> ArchiveReport(Report report)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            if (!report.IsStatus(ReportStatusType.Closed))
                throw new NoPermissionsException("Report status is not closed");

            report.Archive();

            return await database.ReportRepository.Update(report);
        }

        public async Task<ArchiveReportsResult> ArchiveReports()
        {
            var reportsToArchive = (await database.ReportRepository.GetWhere(new SqlBuilder()
                    .Append($"DATE_ADD(dateClosed, INTERVAL {Constants.ReportArchivizationTimeInDays} DAY)")
                    .Lesser
                    .Append("NOW()")
                    .And
                    .Append("status")
                    .Equals
                    .Append((int) ReportStatusType.Closed)
                    .Build()
                    .Query))
                .ToList();

            reportsToArchive.ForEach(r => r.Archive());

            return new ArchiveReportsResult(reportsToArchive.Count,
                await database.ReportRepository.UpdateRange(reportsToArchive));
        }

        public async Task<bool> MoveReportAssignment(Report report, int userId)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            report.AssigneTo(userId);

            return await database.ReportRepository.Update(report);
        }

        public async Task<bool> AssignAwaitingReports(ReportCategoryType reportCategoryType, bool isPrivate)
        {
            var allowedAssignees = (await GetReportsAllowedAssignees(reportCategoryType, isPrivate))
                .Where(u => u.AssignedReports.Count < Constants.MaximumAssignedReportsCount);

            var awaitingReports = await database.ReportRepository.GetWhere(new SqlBuilder()
                .Append("status").Equals.Append((int) ReportStatusType.Awaiting)
                .And.Append("categoryType").Equals.Append((int) reportCategoryType)
                .And.Append("isPrivate").Equals.Append(isPrivate ? 1 : 0)
                .Build().Query);

            foreach (var awaitingReport in awaitingReports)
            {
                awaitingReport.AssigneTo(allowedAssignees
                    .Aggregate(
                        (currentMin, u) =>
                            (currentMin == null || u.AssignedReports.Count < currentMin.AssignedReports.Count
                                ? u
                                : currentMin))
                    .Id);

                awaitingReport.ChangeStatus(ReportStatusType.Assigned);
            }

            return await database.ReportRepository.UpdateRange(awaitingReports);
        }

        public async Task<TogglePrivacyReportResult> TogglePrivacyReport(Report report)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            report.TogglePrivacy();

            return new TogglePrivacyReportResult(IsPrivate: report.Private, IsSucceeded:
                await database.ReportRepository.Update(report));
        }
    }
}