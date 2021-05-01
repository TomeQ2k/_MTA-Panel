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
    public class FetchArchivedReportsQuery : IRequestHandler<FetchArchivedReportsRequest,
        FetchArchivedReportsResponse>
    {
        private readonly IReadOnlyReportService reportService;
        private readonly IMapper mapper;
        private readonly IHttpContextWriter httpContextWriter;

        public FetchArchivedReportsQuery(IReadOnlyReportService reportService, IMapper mapper,
            IHttpContextWriter httpContextWriter)
        {
            this.reportService = reportService;
            this.mapper = mapper;
            this.httpContextWriter = httpContextWriter;
        }

        public async Task<FetchArchivedReportsResponse> Handle(
            FetchArchivedReportsRequest request, CancellationToken cancellationToken)
        {
            var archivedReports = await reportService.FetchArchivedReports(request);

            httpContextWriter.AddPagination(archivedReports.CurrentPage, archivedReports.PageSize,
                archivedReports.TotalCount,
                archivedReports.TotalPages);

            return new FetchArchivedReportsResponse
            {
                Reports = mapper.Map<IEnumerable<ReportListDto>>(archivedReports)
            };
        }
    }
}