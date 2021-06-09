using System;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportService : IReportService
    {
        private readonly IDatabase database;
        private readonly IReportImageService reportImageService;
        private readonly IHttpContextReader httpContextReader;

        public ReportService(IDatabase database, IReportImageService reportImageService,
            IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.reportImageService = reportImageService;
            this.httpContextReader = httpContextReader;
        }

        #region ReadOnly

        public async Task<Report> GetReport(string reportId)
            => await database.ReportRepository.GetReport(reportId)
               ?? throw new EntityNotFoundException("Report not found");

        public async Task<PagedList<Report>> FetchAllReports(ReportFiltersParams request,
            ReportCategoryType[] reportCategoryTypes, bool isOwner)
            => (await database.ReportRepository.FetchAllReports(request, reportCategoryTypes, isOwner))
                .ToPagedList(request.PageNumber, request.PageSize);

        public async Task<PagedList<Report>> FetchArchivedReports(ArchivedReportFiltersParams request)
            => (await database.ReportRepository.FetchArchivedReports(request)).ToPagedList(request.PageNumber,
                request.PageSize);

        public async Task<PagedList<Report>> FetchReportsByUser(UserReportFiltersParams request)
            => (await database.ReportRepository.FetchReportsByUser(request, httpContextReader.CurrentUserId))
                .ToPagedList(request.PageNumber, request.PageSize);

        #endregion

        #region Write

        public async Task<Report> CreateOtherReport(CreateOtherReportRequest request)
        {
            var baseReport = await CreateBaseReport(request.Subject, request.Content, request.IsPrivate,
                request.EventDate, request.Type);

            if (!await database.ReportRepository.Insert(baseReport, false))
                throw new DatabaseException();

            await reportImageService.UploadReportImages(httpContextReader.CurrentUserId,
                baseReport,
                request.Images);

            return baseReport;
        }

        public async Task<UserReport> CreateUserReport(CreateUserReportRequest request)
        {
            var baseReport = await CreateBaseReport(request.Subject, request.Content, request.IsPrivate,
                request.EventDate, ReportCategoryType.User);

            if (await database.UserRepository.FindById(request.UserToReportId) == null)
                throw new EntityNotFoundException("User to report not found");

            var userReport = UserReport.Create(baseReport.Id, request.UserToReportId, request.WitnessId);
            userReport.SetReport(baseReport);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (!await database.ReportRepository.Insert(baseReport, false))
                    throw new DatabaseException();

                if (!await database.UserReportRepository.Insert(userReport, false))
                    throw new DatabaseException();

                transaction.Complete();
            }

            await reportImageService.UploadReportImages(httpContextReader.CurrentUserId,
                baseReport,
                request.Images);

            return userReport;
        }


        public async Task<PenaltyReport> CreatePenaltyReport(CreatePenaltyReportRequest request)
        {
            var baseReport = await CreateBaseReport(request.Subject, request.Content, request.IsPrivate,
                request.EventDate, ReportCategoryType.Penalty);

            var penaltyReport = PenaltyReport.Create(baseReport.Id, request.BanId, request.PenaltyId);
            penaltyReport.SetReport(baseReport);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (!await database.ReportRepository.Insert(baseReport, false))
                    throw new DatabaseException();

                if (!await database.PenaltyReportRepository.Insert(penaltyReport, false))
                    throw new DatabaseException();

                transaction.Complete();
            }

            await reportImageService.UploadReportImages(httpContextReader.CurrentUserId,
                baseReport,
                request.Images);

            return penaltyReport;
        }

        public async Task<BugReport> CreateBugReport(CreateBugReportRequest request)
        {
            var baseReport = await CreateBaseReport(request.Subject, request.Content, request.IsPrivate,
                request.EventDate, ReportCategoryType.Bug);

            var bugReport = BugReport.Create(baseReport.Id, request.BugType, request.AdditionalInfo);
            bugReport.SetReport(baseReport);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (!await database.ReportRepository.Insert(baseReport, false))
                    throw new DatabaseException();

                if (!await database.BugReportRepository.Insert(bugReport, false))
                    throw new DatabaseException();

                transaction.Complete();
            }

            await reportImageService.UploadReportImages(httpContextReader.CurrentUserId,
                baseReport,
                request.Images);

            return bugReport;
        }

        #endregion

        #region private

        private async Task<Report> CreateBaseReport(string subject, string content, bool isPrivate, DateTime? eventDate,
            ReportCategoryType categoryType)
            => await database.UserRepository.FindById(httpContextReader.CurrentUserId) != null
                ? new ReportBuilder()
                    .CreatedBy(httpContextReader.CurrentUserId)
                    .SetSubject(subject)
                    .SetContent(content)
                    .SetPrivacy(isPrivate)
                    .SetEventDate(eventDate)
                    .SetCategoryType(categoryType)
                    .Build()
                : throw new EntityNotFoundException("User not found");

        #endregion
    }
}