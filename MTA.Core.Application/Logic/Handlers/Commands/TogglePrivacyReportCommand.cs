using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class TogglePrivacyReportCommand : IRequestHandler<TogglePrivacyReportRequest, TogglePrivacyReportResponse>
    {
        private readonly IReportManager reportManager;
        private readonly IReportValidationHub reportValidationHub;

        public TogglePrivacyReportCommand(IReportManager reportManager, IReportValidationHub reportValidationHub)
        {
            this.reportManager = reportManager;
            this.reportValidationHub = reportValidationHub;
        }

        public async Task<TogglePrivacyReportResponse> Handle(TogglePrivacyReportRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.TogglePrivacy)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var result = await reportManager.TogglePrivacyReport(report);

            return result.IsSucceeded
                ? new TogglePrivacyReportResponse
                {
                    IsSucceeded = result.IsSucceeded,
                    IsPrivate = result.IsPrivate
                }
                : throw new CrudException("Toggling private status of report failed");
        }
    }
}