using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record PassRPTestPartOneResponse : BaseResponse
    {
        public bool IsPassed { get; init; }

        public PassRPTestPartOneResponse(Error error = null) : base(error)
        {
        }
    }
}