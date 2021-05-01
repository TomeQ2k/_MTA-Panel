using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record ReviewRPTestResponse : BaseResponse
    {
        public ReviewRPTestResponse(Error error = null) : base(error)
        {
        }
    }
}