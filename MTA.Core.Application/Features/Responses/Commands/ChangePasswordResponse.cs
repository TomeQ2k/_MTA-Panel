using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ChangePasswordResponse : BaseResponse
    {
        public ChangePasswordResponse(Error error = null) : base(error)
        {
        }
    }
}