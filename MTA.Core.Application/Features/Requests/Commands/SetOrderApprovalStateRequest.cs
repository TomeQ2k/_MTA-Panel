using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record SetOrderApprovalStateRequest : IRequest<SetOrderApprovalStateResponse>
    {
        public string OrderId { get; init; }
        public StateType ApprovalState { get; init; }
        public string AdminNote { get; init; }
    }

    public class SetOrderApprovalStateRequestValidator : AbstractValidator<SetOrderApprovalStateRequest>
    {
        public SetOrderApprovalStateRequestValidator()
        {
            RuleFor(x => x.OrderId).NotNull();
            RuleFor(x => x.ApprovalState).IsInEnum();
            RuleFor(x => x.AdminNote).MaximumLength(Constants.MaximumAdminNoteLength);
        }
    }
}