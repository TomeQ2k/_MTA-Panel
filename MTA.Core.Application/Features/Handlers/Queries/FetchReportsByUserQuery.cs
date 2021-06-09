using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class FetchReportsByUserQuery : IRequestHandler<FetchReportsByUserRequest,
        FetchReportsByUserResponse>
    {
        private readonly IReadOnlyReportService reportService;
        private readonly IMapper mapper;
        private readonly IHttpContextWriter httpContextWriter;

        public FetchReportsByUserQuery(IReadOnlyReportService reportService, IMapper mapper,
            IHttpContextWriter httpContextWriter)
        {
            this.reportService = reportService;
            this.mapper = mapper;
            this.httpContextWriter = httpContextWriter;
        }

        public async Task<FetchReportsByUserResponse> Handle(FetchReportsByUserRequest request,
            CancellationToken cancellationToken)
        {
            var reports = await reportService.FetchReportsByUser(request);

            httpContextWriter.AddPagination(reports.CurrentPage, reports.PageSize, reports.TotalCount,
                reports.TotalPages);

            return new FetchReportsByUserResponse
            {
                Reports = mapper.Map<IEnumerable<ReportListDto>>(reports)
            };
        }
    }
}