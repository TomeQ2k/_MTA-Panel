using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record VerifyResetPasswordResponse : BaseResponse
    {
        public VerifyResetPasswordResponse(Error error = null) : base(error)
        {
        }
    }
}