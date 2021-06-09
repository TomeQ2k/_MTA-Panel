using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyReportService
    {
        Task<Report> GetReport(string reportId);

        Task<PagedList<Report>> FetchAllReports(ReportFiltersParams request, ReportCategoryType[] reportCategoryTypes,
            bool isOwner);

        Task<PagedList<Report>> FetchArchivedReports(ArchivedReportFiltersParams request);
        Task<PagedList<Report>> FetchReportsByUser(UserReportFiltersParams request);
    }
}