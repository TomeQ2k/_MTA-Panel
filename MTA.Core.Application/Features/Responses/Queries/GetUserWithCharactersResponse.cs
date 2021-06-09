using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetUserWithCharactersResponse : BaseResponse
    {
        public UserWithCharactersDto User { get; init; }

        public GetUserWithCharactersResponse(Error error = null) : base(error)
        {
        }
    }
}