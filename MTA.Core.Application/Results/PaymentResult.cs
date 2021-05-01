using System.Collections.Generic;
using System.Net;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Results
{
    public record PaymentResult
    {
        public string OrderId { get; init; }
        public IEnumerable<OrderLink> Links { get; init; }
        public HttpStatusCode StatusCode { get; init; }
    }
}