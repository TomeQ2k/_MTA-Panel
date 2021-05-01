using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetApiLogsQuery : IRequestHandler<GetApiLogsRequest, GetApiLogsResponse>
    {
        private readonly ILogReader logReader;
        private readonly IHttpContextWriter httpContextWriter;

        public GetApiLogsQuery(ILogReader logReader, IHttpContextWriter httpContextWriter)
        {
            this.logReader = logReader;
            this.httpContextWriter = httpContextWriter;
        }

        public async Task<GetApiLogsResponse> Handle(GetApiLogsRequest request, CancellationToken cancellationToken)
        {
            var apiLogs = await logReader.GetApiLogsFromFile(request);

            httpContextWriter.AddPagination(apiLogs.CurrentPage, apiLogs.PageSize, apiLogs.TotalCount,
                apiLogs.TotalPages);

            return new GetApiLogsResponse {ApiLogs = apiLogs};
        }
    }
}