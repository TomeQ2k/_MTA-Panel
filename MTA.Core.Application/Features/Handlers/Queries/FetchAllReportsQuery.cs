using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class FetchAllReportsQuery :
        IRequestHandler<FetchAllReportsRequest, FetchAllReportsResponse>
    {
        private readonly IReadOnlyReportService reportService;
        private readonly IReportValidationService reportValidationService;
        private readonly IMapper mapper;
        private readonly IHttpContextWriter httpContextWriter;

        public FetchAllReportsQuery(IReadOnlyReportService reportService,
            IReportValidationService reportValidationService, IMapper mapper, IHttpContextWriter httpContextWriter)
        {
            this.reportService = reportService;
            this.reportValidationService = reportValidationService;
            this.mapper = mapper;
            this.httpContextWriter = httpContextWriter;
        }

        public async Task<FetchAllReportsResponse> Handle(FetchAllReportsRequest request,
            CancellationToken cancellationToken)
        {
            var findCategoriesToReadResult = await reportValidationService.FindCategoriesToRead();

            if (request.CategoryType != ReportCategoryType.All &&
                !findCategoriesToReadResult.ReportCategoryTypes.Contains(request.CategoryType))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var reports = await reportService.FetchAllReports(request, findCategoriesToReadResult.ReportCategoryTypes,
                findCategoriesToReadResult.IsOwner);

            httpContextWriter.AddPagination(reports.CurrentPage, reports.PageSize, reports.TotalCount,
                reports.TotalPages);

            return new FetchAllReportsResponse
            {
                Reports = mapper.Map<IEnumerable<ReportListDto>>(reports)
            };
        }
    }
}