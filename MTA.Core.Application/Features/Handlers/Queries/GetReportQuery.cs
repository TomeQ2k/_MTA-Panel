using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetReportQuery : IRequestHandler<GetReportRequest, GetReportResponse>
    {
        private readonly IReadOnlyReportService reportService;
        private readonly IMapper mapper;

        public GetReportQuery(IReadOnlyReportService reportService, IMapper mapper)
        {
            this.reportService = reportService;
            this.mapper = mapper;
        }

        public async Task<GetReportResponse> Handle(GetReportRequest request, CancellationToken cancellationToken)
            => new GetReportResponse {Report = mapper.Map<ReportDto>(await reportService.GetReport(request.ReportId))};
    }
}