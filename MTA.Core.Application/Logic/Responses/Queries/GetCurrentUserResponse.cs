using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetCurrentUserResponse : BaseResponse
    {
        public UserWithCharactersDto User { get; init; }

        public GetCurrentUserResponse(Error error = null) : base(error)
        {
        }
    }
}