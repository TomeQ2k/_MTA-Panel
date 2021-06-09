using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ReviewRPTestResponse : BaseResponse
    {
        public ReviewRPTestResponse(Error error = null) : base(error)
        {
        }
    }
}