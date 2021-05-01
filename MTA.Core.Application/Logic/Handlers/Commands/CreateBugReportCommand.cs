using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class CreateBugReportCommand : IRequestHandler<CreateBugReportRequest, CreateBugReportResponse>
    {
        private readonly IReportService reportService;
        private readonly IReportManager reportManager;
        private readonly IMapper mapper;

        public CreateBugReportCommand(IReportService reportService, IReportManager reportManager, IMapper mapper)
        {
            this.reportService = reportService;
            this.reportManager = reportManager;
            this.mapper = mapper;
        }

        public async Task<CreateBugReportResponse> Handle(CreateBugReportRequest request,
            CancellationToken cancellationToken)
        {
            var createdReport = await reportService.CreateBugReport(request) ??
                                throw new ServerException("Error occured during creating Report");

            await reportManager.AssignAwaitingReports(ReportCategoryType.Bug, request.IsPrivate);

            return new CreateBugReportResponse
            {
                BugReport = mapper.Map<BugReportDto>(createdReport)
            };
        }
    }
}