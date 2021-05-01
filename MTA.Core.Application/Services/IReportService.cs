using System.Threading.Tasks;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportService : IReadOnlyReportService
    {
        Task<Report> CreateOtherReport(CreateOtherReportRequest request);
        Task<UserReport> CreateUserReport(CreateUserReportRequest request);
        Task<PenaltyReport> CreatePenaltyReport(CreatePenaltyReportRequest request);
        Task<BugReport> CreateBugReport(CreateBugReportRequest request);
    }
}