using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetUsersByAdminResponse : BaseResponse
    {
        public IEnumerable<UserAdminListDto> Users { get; init; }

        public GetUsersByAdminResponse(Error error = null) : base(error)
        {
        }
    }
}