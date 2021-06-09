using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class MoveReportAssignmentCommand :
        IRequestHandler<MoveReportAssignmentRequest, MoveReportAssignmentResponse>
    {
        private readonly IReportManager reportManager;
        private readonly IReportValidationHub reportValidationHub;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public MoveReportAssignmentCommand(IReportManager reportManager, IReportValidationHub reportValidationHub,
            IHttpContextReader httpContextReader, INotifier notifier, IHubManager<NotifierHub> hubManager,
            IMapper mapper)
        {
            this.reportManager = reportManager;
            this.reportValidationHub = reportValidationHub;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<MoveReportAssignmentResponse> Handle(MoveReportAssignmentRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.MoveAssignment)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            if (!(await reportManager.GetReportsAllowedAssignees((ReportCategoryType) report.CategoryType,
                isPrivate: report.Private)).Any(a => a.Id == request.NewAssigneeId))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            if (await reportManager.MoveReportAssignment(report, request.NewAssigneeId))
            {
                var membersGroup = new List<int> {report.CreatorId, request.NewAssigneeId};

                var notifications = await notifier.PushToGroup(
                    $"Report {report.Subject} assignment has been moved to another admin",
                    membersGroup);

                foreach (var memberId in membersGroup)
                    await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, memberId,
                        mapper.Map<IEnumerable<NotificationDto>>(notifications));

                return (MoveReportAssignmentResponse) new MoveReportAssignmentResponse().LogInformation(
                    $"Assignee #{httpContextReader.CurrentUserId} move report #{report.Id} assignment to user #{request.NewAssigneeId}");
            }

            throw new CrudException("Toggling private status of report failed");
        }
    }
}