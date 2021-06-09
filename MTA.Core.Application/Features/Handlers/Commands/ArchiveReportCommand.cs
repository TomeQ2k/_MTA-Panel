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
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class ArchiveReportCommand : IRequestHandler<ArchiveReportRequest, ArchiveReportResponse>
    {
        private readonly IReportManager reportManager;
        private readonly IReportValidationHub reportValidationHub;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public ArchiveReportCommand(IReportManager reportManager, IReportValidationHub reportValidationHub,
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

        public async Task<ArchiveReportResponse> Handle(ArchiveReportRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.ArchiveReport)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            if (await reportManager.ArchiveReport(report))
            {
                var membersGroup =
                    ReportManagerUtils.GenerateReportMembersGroup(report, httpContextReader.CurrentUserId);

                var notifications = await notifier.PushToGroup(
                    $"Report {report.Subject} has been archived",
                    membersGroup);

                foreach (var memberId in membersGroup)
                    await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, memberId,
                        mapper.Map<IEnumerable<NotificationDto>>(notifications));

                return (ArchiveReportResponse) new ArchiveReportResponse().LogInformation(
                    $"Report #{report.Id} has been archived");
            }

            throw new CrudException("Archiving report failed");
        }
    }
}