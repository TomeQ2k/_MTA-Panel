using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetUserPurchasesResponse : BaseResponse
    {
        public IEnumerable<PurchaseDto> Purchases { get; init; }

        public GetUserPurchasesResponse(Error error = null) : base(error)
        {
        }
    }
}