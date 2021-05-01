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
    public class AcceptReportAssignmentCommand :
        IRequestHandler<AcceptReportAssignmentRequest, AcceptReportAssignmentResponse>
    {
        private readonly IReportManager reportManager;
        private readonly IReportValidationHub reportValidationHub;

        public AcceptReportAssignmentCommand(IReportManager reportManager, IReportValidationHub reportValidationHub)
        {
            this.reportManager = reportManager;
            this.reportValidationHub = reportValidationHub;
        }

        public async Task<AcceptReportAssignmentResponse> Handle(AcceptReportAssignmentRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.AcceptAssignment)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var reportAssignmentResult = await reportManager.AcceptReportAssignment(report);

            return reportAssignmentResult.IsSucceeded
                ? new AcceptReportAssignmentResponse
                {
                    IsSucceeded = reportAssignmentResult.IsSucceeded, 
                    IsAccepted = reportAssignmentResult.IsAccepted
                }
                : throw new CrudException("Accept report assignment failed");
        }
    }
}