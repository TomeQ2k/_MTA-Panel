using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetHomepageStatsRequest : IRequest<GetHomepageStatsResponse>
    {
    }
}