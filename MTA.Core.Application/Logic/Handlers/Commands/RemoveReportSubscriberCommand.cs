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
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class RemoveReportSubscriberCommand :
        IRequestHandler<RemoveReportSubscriberRequest, RemoveReportSubscriberResponse>
    {
        private readonly IReportSubscriberService reportSubscriberService;
        private readonly IReportValidationHub reportValidationHub;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public RemoveReportSubscriberCommand(IReportSubscriberService reportSubscriberService,
            IReportValidationHub reportValidationHub, IHttpContextReader httpContextReader, INotifier notifier,
            IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.reportSubscriberService = reportSubscriberService;
            this.reportValidationHub = reportValidationHub;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<RemoveReportSubscriberResponse> Handle(RemoveReportSubscriberRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.AddComment)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            if (await reportSubscriberService.RemoveSubscriber(report, request.UserId))
            {
                var notification = await notifier.Push(
                    $"You were removed from subscribers in report: {report.Subject}",
                    request.UserId);

                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, request.UserId,
                    mapper.Map<NotificationDto>(notification));

                return (RemoveReportSubscriberResponse) new RemoveReportSubscriberResponse().LogInformation(
                    $"Assignee #{httpContextReader.CurrentUserId} has removed subscriber #{request.UserId} in report #{report.Id}");
            }

            throw new CrudException("Removing report subscriber failed");
        }
    }
}