using MediatR;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetArticlesRequest : IRequest<GetArticlesResponse>
    {
        public int Limit { get; init; } = Constants.ArticlesCount;
    }
}