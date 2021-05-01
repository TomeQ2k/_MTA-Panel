using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetUsersByUsernameResponse : BaseResponse
    {
        public IEnumerable<UserListDto> Users { get; init; }

        public GetUsersByUsernameResponse(Error error = null) : base(error)
        {
        }
    }
}