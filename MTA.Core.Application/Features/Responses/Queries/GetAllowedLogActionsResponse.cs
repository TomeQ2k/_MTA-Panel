using System.Collections.Generic;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetAllowedLogActionsResponse : BaseResponse
    {
        public IEnumerable<LogAction> AllowedLogActions { get; init; }

        public GetAllowedLogActionsResponse(Error error = null) : base(error)
        {
        }
    }
}