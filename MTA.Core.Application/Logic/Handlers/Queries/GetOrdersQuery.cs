using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetOrdersQuery : IRequestHandler<GetOrdersRequest, GetOrdersResponse>
    {
        private readonly IReadOnlyOrderService orderService;
        private readonly IHttpContextWriter httpContextWriter;
        private readonly IMapper mapper;

        public GetOrdersQuery(IReadOnlyOrderService orderService, IHttpContextWriter httpContextWriter, IMapper mapper)
        {
            this.orderService = orderService;
            this.httpContextWriter = httpContextWriter;
            this.mapper = mapper;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderService.GetOrders(request);

            httpContextWriter.AddPagination(orders.CurrentPage, orders.PageSize, orders.TotalCount, orders.TotalPages);

            return new GetOrdersResponse {Orders = mapper.Map<IEnumerable<OrderDto>>(orders)};
        }
    }
}