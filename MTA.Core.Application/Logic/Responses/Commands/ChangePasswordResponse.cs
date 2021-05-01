using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record ChangePasswordResponse : BaseResponse
    {
        public ChangePasswordResponse(Error error = null) : base(error)
        {
        }
    }
}