using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ConfirmAccountResponse : BaseResponse
    {
        public ConfirmAccountResponse(Error error = null) : base(error)
        {
        }
    }
}