using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetReportsAllowedAssigneesResponse : BaseResponse
    {
        public IEnumerable<UserAssigneeDto> AllowedAssignees { get; init; }

        public GetReportsAllowedAssigneesResponse(Error error = null) : base(error)
        {
        }
    }
}