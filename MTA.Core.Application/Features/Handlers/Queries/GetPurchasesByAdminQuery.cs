using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetPurchasesByAdminQuery : IRequestHandler<GetPurchasesByAdminRequest, GetPurchasesByAdminResponse>
    {
        private readonly IPurchaseService purchaseService;
        private readonly IHttpContextWriter httpContextWriter;
        private readonly IMapper mapper;

        public GetPurchasesByAdminQuery(IPurchaseService purchaseService, IHttpContextWriter httpContextWriter,
            IMapper mapper)
        {
            this.purchaseService = purchaseService;
            this.httpContextWriter = httpContextWriter;
            this.mapper = mapper;
        }

        public async Task<GetPurchasesByAdminResponse> Handle(GetPurchasesByAdminRequest request,
            CancellationToken cancellationToken)
        {
            var purchases = await purchaseService.GetPurchasesByAdmin(request);

            httpContextWriter.AddPagination(purchases.CurrentPage, purchases.PageSize, purchases.TotalCount,
                purchases.TotalPages);

            return new GetPurchasesByAdminResponse
            {
                Purchases = mapper.Map<IEnumerable<PurchaseDto>>(purchases)
            };
        }
    }
}