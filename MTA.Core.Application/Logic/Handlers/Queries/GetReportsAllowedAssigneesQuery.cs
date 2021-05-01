using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetReportsAllowedAssigneesQuery
        : IRequestHandler<GetReportsAllowedAssigneesRequest, GetReportsAllowedAssigneesResponse>
    {
        private readonly IReadOnlyReportManager reportManager;
        private readonly IMapper mapper;

        public GetReportsAllowedAssigneesQuery(IReadOnlyReportManager reportManager, IMapper mapper)
        {
            this.reportManager = reportManager;
            this.mapper = mapper;
        }

        public async Task<GetReportsAllowedAssigneesResponse> Handle(GetReportsAllowedAssigneesRequest request,
            CancellationToken cancellationToken)
            => new GetReportsAllowedAssigneesResponse
            {
                AllowedAssignees = mapper.Map<IEnumerable<UserAssigneeDto>>(
                    await reportManager.GetReportsAllowedAssignees(request.ReportCategoryType, request.IsPrivate))
            };
    }
}