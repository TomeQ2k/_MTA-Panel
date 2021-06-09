using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetOrdersResponse : BaseResponse
    {
        public IEnumerable<OrderDto> Orders { get; init; }

        public GetOrdersResponse(Error error = null) : base(error)
        {
        }
    }
}