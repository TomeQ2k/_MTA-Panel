using System.Collections.Generic;
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
using MTA.Core.Application.ServicesUtils;
using MTA.Core.Application.SignalR;
using MTA.Core.Application.SignalR.Hubs;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class AddReportCommentCommand : IRequestHandler<AddReportCommentRequest, AddReportCommentResponse>
    {
        private readonly IReportCommentService reportCommentService;
        private readonly IReportValidationHub reportValidationHub;
        private readonly IReportManager reportManager;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public AddReportCommentCommand(IReportCommentService reportCommentService,
            IReportValidationHub reportValidationHub, IReportManager reportManager, INotifier notifier,
            IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.reportCommentService = reportCommentService;
            this.reportValidationHub = reportValidationHub;
            this.reportManager = reportManager;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<AddReportCommentResponse> Handle(AddReportCommentRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.AddComment)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var addedReportComment = await reportCommentService.AddComment(request with {IsPrivate = report.Private})
                                     ?? throw new CrudException("Adding report comment failed");

            if (ReportStatusTypeSmartEnum.FromValue(report.Status)
                .ShouldBeOpened(report, addedReportComment.UserId))
                await reportManager.ChangeStatus(ReportStatusType.Opened, report);

            var membersGroup = ReportManagerUtils.GenerateReportMembersGroup(report, addedReportComment.UserId);

            var notifications = await notifier.PushToGroup(
                $"Someone responded to your report: {report.Subject}", membersGroup);

            foreach (var memberId in membersGroup)
                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, memberId,
                    mapper.Map<IEnumerable<NotificationDto>>(notifications));

            return (AddReportCommentResponse) new AddReportCommentResponse
                    {ReportComment = mapper.Map<ReportCommentDto>(addedReportComment)}
                .LogInformation(
                    $"Member #{addedReportComment.UserId} of report #{report.Id} added comment #{addedReportComment.Id}");
        }
    }
}