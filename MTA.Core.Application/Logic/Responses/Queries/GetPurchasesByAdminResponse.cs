using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetPurchasesByAdminResponse : BaseResponse
    {
        public IEnumerable<PurchaseDto> Purchases { get; init; }

        public GetPurchasesByAdminResponse(Error error = null) : base(error)
        {
        }
    }
}