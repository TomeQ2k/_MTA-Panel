using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Logging;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetAllowedLogActionsQuery : IRequestHandler<GetAllowedLogActionsRequest, GetAllowedLogActionsResponse>
    {
        private readonly ILogReaderHelper logReaderHelper;

        public GetAllowedLogActionsQuery(ILogReaderHelper logReaderHelper)
        {
            this.logReaderHelper = logReaderHelper;
        }

        public async Task<GetAllowedLogActionsResponse> Handle(GetAllowedLogActionsRequest request,
            CancellationToken cancellationToken)
            => new GetAllowedLogActionsResponse {AllowedLogActions = await logReaderHelper.GetAllowedLogActions()};
    }
}