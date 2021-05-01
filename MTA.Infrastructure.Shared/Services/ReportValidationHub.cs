using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportValidationHub : IReportValidationHub
    {
        private readonly IReportValidationService reportValidationService;
        private readonly IReadOnlyReportService reportService;
        private readonly IHttpContextReader httpContextReader;

        public ReportValidationHub(IReportValidationService reportValidationService,
            IReadOnlyReportService reportService, IHttpContextReader httpContextReader)
        {
            this.reportValidationService = reportValidationService;
            this.reportService = reportService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<Report> ValidateAndReturnReport(string reportId, params ReportPermission[] permissions)
        {
            var report = await reportService.GetReport(reportId) ??
                         throw new EntityNotFoundException("Report not found");

            return await reportValidationService.IsUserReportMember(httpContextReader.CurrentUserId, report) &&
                   await reportValidationService.ValidatePermissions(httpContextReader.CurrentUserId, report,
                       permissions)
                ? report
                : throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);
        }
    }
}