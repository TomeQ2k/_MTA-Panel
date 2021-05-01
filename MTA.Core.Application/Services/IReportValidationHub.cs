using System.Threading.Tasks;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportValidationHub
    {
        Task<Report> ValidateAndReturnReport(string reportId, params ReportPermission[] permissions);
    }
}