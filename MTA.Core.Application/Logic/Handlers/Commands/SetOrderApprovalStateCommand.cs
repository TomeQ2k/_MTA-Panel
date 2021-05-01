using System;
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

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class SetOrderApprovalStateCommand :
        IRequestHandler<SetOrderApprovalStateRequest, SetOrderApprovalStateResponse>
    {
        private readonly IOrderService orderService;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public SetOrderApprovalStateCommand(IOrderService orderService, IHttpContextReader httpContextReader,
            INotifier notifier, IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.orderService = orderService;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<SetOrderApprovalStateResponse> Handle(SetOrderApprovalStateRequest request,
            CancellationToken cancellationToken)
        {
            var hasOrderReviewed =
                await orderService.SetOrderApprovalState(request.OrderId, request.ApprovalState, request.AdminNote);

            if (!hasOrderReviewed.IsSucceeded)
                throw new PremiumOperationException("Setting approval state of order failed");

            var notification = await notifier.Push(
                $"Approval state of order #{request.OrderId} has been change to {Enum.GetName(request.ApprovalState)}",
                hasOrderReviewed.UserId);

            await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, hasOrderReviewed.UserId,
                mapper.Map<NotificationDto>(notification));

            //TODO: Call LUA

            return (SetOrderApprovalStateResponse) new SetOrderApprovalStateResponse
                    {CurrentApprovalState = request.ApprovalState}
                .LogInformation(
                    $"Admin #{httpContextReader.CurrentUserId} set order #{request.OrderId} approval state to: {(int) request.ApprovalState}");
        }
    }
}