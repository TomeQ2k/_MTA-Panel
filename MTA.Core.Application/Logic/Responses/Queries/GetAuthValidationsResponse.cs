using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetAuthValidationsResponse : BaseResponse
    {
        public bool IsAvailable { get; init; }

        public GetAuthValidationsResponse(Error error = null) : base(error)
        {
        }
    }
}