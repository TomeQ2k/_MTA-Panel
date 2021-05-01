using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class AddReportSubscriberCommand : IRequestHandler<AddReportSubscriberRequest, AddReportSubscriberResponse>
    {
        private readonly IReportSubscriberService reportSubscriberService;
        private readonly IReportValidationHub reportValidationHub;
        private readonly IReportManager reportManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public AddReportSubscriberCommand(IReportSubscriberService reportSubscriberService,
            IReportValidationHub reportValidationHub, IReportManager reportManager,
            IHttpContextReader httpContextReader, INotifier notifier, IHubManager<NotifierHub> hubManager,
            IMapper mapper)
        {
            this.reportSubscriberService = reportSubscriberService;
            this.reportValidationHub = reportValidationHub;
            this.reportManager = reportManager;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<AddReportSubscriberResponse> Handle(AddReportSubscriberRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.AddComment)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var addedReportSubscriber = await reportSubscriberService.AddSubscriber(report, request.UserId) ??
                                        throw new CrudException("Adding report subscriber failed");

            if (ReportStatusTypeSmartEnum.FromValue(report.Status)
                .ShouldBeOpened(report, httpContextReader.CurrentUserId))
                await reportManager.ChangeStatus(ReportStatusType.Opened, report);

            var notification = await notifier.Push(
                $"You were added to subscribers in report: {report.Subject}",
                request.UserId);

            await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, request.UserId,
                mapper.Map<NotificationDto>(notification));

            return (AddReportSubscriberResponse) new AddReportSubscriberResponse
                {ReportSubscriber = mapper.Map<ReportSubscriberDto>(addedReportSubscriber)}.LogInformation(
                $"Assignee #{httpContextReader.CurrentUserId} has added subscriber #{request.UserId} in report #{report.Id}");
        }
    }
}