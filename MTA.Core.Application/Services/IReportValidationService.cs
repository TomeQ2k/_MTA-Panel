using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportValidationService
    {
        Task<bool> ValidatePermissions(int userId, Report report, params ReportPermission[] permissions);
        Task<bool> IsUserReportMember(int userId, Report report);
        Task<bool> ValidateMaxFilesCountAndSizePerUser(int userId, Report report, IEnumerable<IFormFile> images);

        Task<FindCategoriesToReadResult> FindCategoriesToRead();
    }
}