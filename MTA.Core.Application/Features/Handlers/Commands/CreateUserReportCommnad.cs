using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class CreateUserReportCommnad : IRequestHandler<CreateUserReportRequest, CreateUserReportResponse>
    {
        private readonly IReportService reportService;
        private readonly IReportManager reportManager;
        private readonly IMapper mapper;

        public CreateUserReportCommnad(IReportService reportService, IReportManager reportManager, IMapper mapper)
        {
            this.reportService = reportService;
            this.reportManager = reportManager;
            this.mapper = mapper;
        }

        public async Task<CreateUserReportResponse> Handle(CreateUserReportRequest request,
            CancellationToken cancellationToken)
        {
            var createdReport = await reportService.CreateUserReport(request) ??
                                throw new ServerException("Error occured during creating Report");

            await reportManager.AssignAwaitingReports(ReportCategoryType.User, request.IsPrivate);

            return new CreateUserReportResponse
            {
                UserReport = mapper.Map<UserReportDto>(createdReport)
            };
        }
    }
}