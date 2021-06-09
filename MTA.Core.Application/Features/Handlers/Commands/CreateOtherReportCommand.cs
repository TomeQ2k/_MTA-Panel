using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class CreateOtherReportCommand : IRequestHandler<CreateOtherReportRequest, CreateOtherReportResponse>
    {
        private readonly IReportService reportService;
        private readonly IReportManager reportManager;
        private readonly IMapper mapper;

        public CreateOtherReportCommand(IReportService reportService, IReportManager reportManager, IMapper mapper)
        {
            this.reportService = reportService;
            this.reportManager = reportManager;
            this.mapper = mapper;
        }

        public async Task<CreateOtherReportResponse> Handle(CreateOtherReportRequest request,
            CancellationToken cancellationToken)
        {
            var createdReport = await reportService.CreateOtherReport(request) ??
                                throw new ServerException("Error occured during creating Report");

            await reportManager.AssignAwaitingReports(request.Type, request.IsPrivate);

            return new CreateOtherReportResponse
            {
                Report = mapper.Map<ReportDto>(createdReport)
            };
        }
    }
}