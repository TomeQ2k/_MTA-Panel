using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record SetOrderApprovalStateResponse : BaseResponse
    {
        public StateType CurrentApprovalState { get; init; }

        public SetOrderApprovalStateResponse(Error error = null) : base(error)
        {
        }
    }
}