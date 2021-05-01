using MediatR;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetOrdersRequest : OrderFiltersParams, IRequest<GetOrdersResponse>
    {
    }
}