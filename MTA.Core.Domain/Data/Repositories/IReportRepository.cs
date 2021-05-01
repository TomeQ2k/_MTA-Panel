using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<Report> GetReport(string reportId);

        Task<IEnumerable<Report>> FetchAllReports(IReportFiltersParams request,
            ReportCategoryType[] reportCategoryTypes, bool isOwner);

        Task<IEnumerable<Report>> FetchArchivedReports(IArchivedReportFiltersParams request);
        Task<IEnumerable<Report>> FetchReportsByUser(IUserReportFiltersParams request, int userId);
    }
}