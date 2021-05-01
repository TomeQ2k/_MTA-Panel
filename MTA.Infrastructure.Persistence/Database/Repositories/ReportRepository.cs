using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<Report> GetReport(string reportId)
        {
            var cacheReportDictionary = new Dictionary<string, Report>();
            var cacheReportCommentsDictionary = new Dictionary<string, ReportComment>();
            var cacheReportSubscriberDictionary = new Dictionary<(string, int), ReportSubscriber>();
            var cacheReportImageDictionary = new Dictionary<string, ReportImage>();

            var siema = new SqlBuilder()
                .Select()
                .From(Table).As("r")
                .LeftJoin("r.id",
                    new(RepositoryDictionary.FindTable(typeof(IRepository<ReportComment>)), "reportId"))
                .LeftJoin("r.id",
                    new(RepositoryDictionary.FindTable(typeof(IRepository<ReportSubscriber>)), "reportId"),
                    "rs")
                .LeftJoin("r.id",
                    new(RepositoryDictionary.FindTable(typeof(IRepository<ReportImage>)), "reportId"),
                    "ri")
                .LeftJoin("r.id",
                    new(RepositoryDictionary.FindTable(typeof(IRepository<BugReport>)), "reportId"), "br")
                .LeftJoin("r.id",
                    new(RepositoryDictionary.FindTable(typeof(IRepository<PenaltyReport>)), "reportId"), "pr")
                .LeftJoin("r.id",
                    new(RepositoryDictionary.FindTable(typeof(IRepository<UserReport>)), "reportId"), "ur")
                .Where("r.id").Equals.Append($"'{reportId}'")
                .Build();

            return (await QueryJoin<Report, ReportComment, ReportSubscriber, ReportImage, BugReport, PenaltyReport,
                UserReport>(
                siema, (report, reportComment, reportSubscriber, reportImage, bugReport, penaltyReport,
                    userReport) =>
                {
                    Report reportEntry;
                    ReportComment reportCommentEntry;
                    ReportSubscriber reportSubscriberEntry;
                    ReportImage reportImageEntry;

                    if (!cacheReportDictionary.TryGetValue(report.Id, out reportEntry))
                    {
                        reportEntry = report;
                        cacheReportDictionary.Add(reportEntry.Id, reportEntry);
                    }

                    if (reportComment != null &&
                        !cacheReportCommentsDictionary.TryGetValue(reportComment.Id, out reportCommentEntry))
                    {
                        reportCommentEntry = reportComment;
                        reportEntry.ReportComments.Add(reportCommentEntry);

                        cacheReportCommentsDictionary.Add(reportCommentEntry.Id, reportCommentEntry);
                    }

                    if (reportSubscriber != null &&
                        !cacheReportSubscriberDictionary.TryGetValue(
                            (reportSubscriber.ReportId, reportSubscriber.UserId),
                            out reportSubscriberEntry))
                    {
                        reportSubscriberEntry = reportSubscriber;
                        reportEntry.ReportSubscribers.Add(reportSubscriberEntry);

                        cacheReportSubscriberDictionary.Add(
                            (reportSubscriberEntry.ReportId, reportSubscriberEntry.UserId),
                            reportSubscriberEntry);
                    }

                    if (reportImage != null &&
                        !cacheReportImageDictionary.TryGetValue(reportImage.Id, out reportImageEntry))
                    {
                        reportImageEntry = reportImage;
                        reportEntry.ReportImages.Add(reportImageEntry);

                        cacheReportImageDictionary.Add(reportImageEntry.Id, reportImageEntry);
                    }

                    if (bugReport != null)
                        reportEntry.SetBugReport(bugReport);
                    if (penaltyReport != null)
                        reportEntry.SetPenaltyReport(penaltyReport);
                    if (userReport != null)
                        reportEntry.SetUserReport(userReport);

                    return reportEntry;
                }, splitOn: "id, id, id, reportId, reportId, reportId")).FirstOrDefault();
        }

        public async Task<IEnumerable<Report>> FetchAllReports(IReportFiltersParams request,
            ReportCategoryType[] reportCategoryTypes, bool isOwner)
            => await Query(new SqlBuilder()
                .Select(
                    "r.id, r.creatorId, r.assigneeId, r.dateCreated, r.dateUpdated, r.subject, r.status, r.isPrivate, r.categoryType, k.username as UserName, kk.username as AdminName")
                .From(Table).As("r")
                .LeftJoin("r.assigneeId",
                    new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                .Join("r.creatorId", new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"),
                    "kk")
                .Where()
                .Case
                .When($"{isOwner} = 1").Then($"r.isPrivate").In(new[] {0, 1})
                .When($"{isOwner} = 0").Then($"r.isPrivate = 0")
                .End.And
                .If(
                    new SqlBuilder().Append($"{(request.AdminId == null ? "null" : request.AdminId)}").IsNotNull
                        .Build().Query,
                    $"r.assigneeId = {(request.AdminId == null ? "null" : request.AdminId)}", "r.assigneeId")
                .And.Case
                .When($"{(int) request.CategoryType} = -1").Then($"r.categoryType")
                .In(reportCategoryTypes.Select(x => (int) x).ToArray())
                .When($"{(int) request.CategoryType} = 0").Then($"r.categoryType = 0")
                .When($"{(int) request.CategoryType} = 1").Then($"r.categoryType = 1")
                .When($"{(int) request.CategoryType} = 2").Then($"r.categoryType = 2")
                .When($"{(int) request.CategoryType} = 3").Then($"r.categoryType = 3")
                .When($"{(int) request.CategoryType} = 4").Then($"r.categoryType = 4")
                .When($"{(int) request.CategoryType} = 5").Then($"r.categoryType = 5")
                .When($"{(int) request.CategoryType} = 6").Then($"r.categoryType = 6")
                .End
                .And.Case
                .When($"{(int) request.StatusType} = -2").Then($"r.status").Between.Append(0).And.Append(4)
                .When($"{(int) request.StatusType} = 0").Then($"r.categoryType = 0")
                .When($"{(int) request.StatusType} = 1").Then($"r.categoryType = 1")
                .When($"{(int) request.StatusType} = 2").Then($"r.categoryType = 2")
                .When($"{(int) request.StatusType} = 3").Then($"r.categoryType = 3")
                .When($"{(int) request.StatusType} = 4").Then($"r.categoryType = 4")
                .End
                .OrderBy("r.dateUpdated", (OrderByType) request.SortType).Build());

        public async Task<IEnumerable<Report>> FetchArchivedReports(IArchivedReportFiltersParams request)
            => await Query(new SqlBuilder()
                .Select(
                    "r.id, r.creatorId, r.assigneeId, r.dateCreated, r.dateUpdated, r.subject, r.status, r.isPrivate, r.categoryType, k.username as UserName, kk.username as AdminName")
                .From(Table).As("r")
                .LeftJoin("r.assigneeId",
                    new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                .Join("r.creatorId", new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"),
                    "kk")
                .Where("r.status").Equals.Append((int) ReportStatusType.Archived).And.If(
                    new SqlBuilder().Append($"{(request.AdminId == null ? "null" : request.AdminId)}").IsNotNull
                        .Build().Query,
                    $"r.assigneeId = {(request.AdminId == null ? "null" : request.AdminId)}", "r.assigneeId")
                .And.Case
                .When($"{(int) request.CategoryType} = -1").Then($"r.categoryType").Between.Append(0).And
                .Append(6)
                .When($"{(int) request.CategoryType} = 0").Then($"r.categoryType = 0")
                .When($"{(int) request.CategoryType} = 1").Then($"r.categoryType = 1")
                .When($"{(int) request.CategoryType} = 2").Then($"r.categoryType = 2")
                .When($"{(int) request.CategoryType} = 3").Then($"r.categoryType = 3")
                .When($"{(int) request.CategoryType} = 4").Then($"r.categoryType = 4")
                .When($"{(int) request.CategoryType} = 5").Then($"r.categoryType = 5")
                .When($"{(int) request.CategoryType} = 6").Then($"r.categoryType = 6")
                .End
                .OrderBy("r.dateUpdated", (OrderByType) request.SortType).Build());

        public async Task<IEnumerable<Report>> FetchReportsByUser(IUserReportFiltersParams request, int userId)
            => await Query(new SqlBuilder()
                .Select(
                    "r.id, r.creatorId, r.assigneeId, r.dateCreated, r.dateUpdated, r.subject, r.status, r.isPrivate, r.categoryType, k.username as UserName, kk.username as AdminName")
                .From(Table).As("r")
                .LeftJoin("r.assigneeId",
                    new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                .Join("r.creatorId", new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"),
                    "kk")
                .Where("kk.id").Equals.Append(userId)
                .And.Case
                .When($"{(int) request.CategoryType} = -1").Then($"r.categoryType").Between.Append(0).And
                .Append(6)
                .When($"{(int) request.CategoryType} = 0").Then($"r.categoryType = 0")
                .When($"{(int) request.CategoryType} = 1").Then($"r.categoryType = 1")
                .When($"{(int) request.CategoryType} = 2").Then($"r.categoryType = 2")
                .When($"{(int) request.CategoryType} = 3").Then($"r.categoryType = 3")
                .When($"{(int) request.CategoryType} = 4").Then($"r.categoryType = 4")
                .When($"{(int) request.CategoryType} = 5").Then($"r.categoryType = 5")
                .When($"{(int) request.CategoryType} = 6").Then($"r.categoryType = 6")
                .End
                .And.Case
                .When($"{(int) request.StatusType} = -2").Then($"r.status").Between.Append(0).And.Append(4)
                .When($"{(int) request.StatusType} = 0").Then($"r.categoryType = 0")
                .When($"{(int) request.StatusType} = 1").Then($"r.categoryType = 1")
                .When($"{(int) request.StatusType} = 2").Then($"r.categoryType = 2")
                .When($"{(int) request.StatusType} = 3").Then($"r.categoryType = 3")
                .When($"{(int) request.StatusType} = 4").Then($"r.categoryType = 4")
                .End
                .OrderBy("r.dateUpdated", (OrderByType) request.SortType).Build());
    }
}