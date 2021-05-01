using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportValidationService : IReportValidationService
    {
        private readonly IDatabase database;
        private readonly IRolesService rolesService;
        private readonly IHttpContextReader httpContextReader;
        private readonly IEnumerable<IReportMember> reportMembers;

        public IConfiguration Configuration { get; }

        public ReportValidationService(IDatabase database, IRolesService rolesService,
            IHttpContextReader httpContextReader, IEnumerable<IReportMember> reportMembers,
            IConfiguration configuration)
        {
            this.database = database;
            this.rolesService = rolesService;
            this.httpContextReader = httpContextReader;
            this.reportMembers = reportMembers;
            Configuration = configuration;
        }

        public async Task<bool> ValidatePermissions(int userId, Report report, params ReportPermission[] permissions)
            => permissions.Select(p => ReportPermissionSmartEnum.FromValue((int) p).IsPermitted(userId, report))
                   .All(p => p.IsPermitted) || await rolesService.IsPermitted(userId, Constants.AllOwnersRoles) ||
               Configuration.IsDev(userId);

        public async Task<bool> IsUserReportMember(int userId, Report report)
            => report.CreatorId == userId
               || report.AssigneeId == userId
               || report.ReportSubscribers.Any(rs => rs.UserId == userId)
               || await rolesService.IsPermitted(userId, Constants.AllOwnersRoles);

        public async Task<bool> ValidateMaxFilesCountAndSizePerUser(int userId, Report report,
            IEnumerable<IFormFile> images)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            var user = await database.UserRepository.FindById(userId) ??
                       throw new EntityNotFoundException("User not found");

            var definedReportMembers = reportMembers.Select(rm => rm.DefineReportMember(report, user))
                .Where(rm => rm != null);

            bool isValidated = false;
            definedReportMembers.ToList()
                .ForEach(rm =>
                    isValidated = isValidated || rm.ValidateImagesForMember(userId, report.ReportImages, images));

            return isValidated;
        }

        public async Task<FindCategoriesToReadResult> FindCategoriesToRead()
        {
            var user = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            RoleType userRole;
            if (user.AdminRole > 0)
                userRole = RoleDictionary.FindRoleTypeByUserRole(new("admin", user.AdminRole));
            else if (user.SupporterRole > 0)
                userRole = RoleDictionary.FindRoleTypeByUserRole(new ColumnValue("supporter", user.SupporterRole));
            else
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            return new FindCategoriesToReadResult(
                ReportCategoryPermissionDictionary.ReportCategoryPermissions[userRole], user.AdminRole >= 3);
        }
    }
}