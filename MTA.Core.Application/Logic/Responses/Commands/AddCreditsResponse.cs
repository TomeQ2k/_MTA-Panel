using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AddCreditsResponse : BaseResponse
    {
        public int CreditsCount { get; init; }

        public AddCreditsResponse(Error error = null) : base(error)
        {
        }
    }
}