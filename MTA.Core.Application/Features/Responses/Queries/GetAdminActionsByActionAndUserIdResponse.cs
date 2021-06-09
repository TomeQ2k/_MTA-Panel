using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetAdminActionsByActionAndUserIdResponse : BaseResponse
    {
        public IEnumerable<AdminActionDto> AdminActions { get; init; }

        public GetAdminActionsByActionAndUserIdResponse(Error error = null) : base(error)
        {
        }
    }
}