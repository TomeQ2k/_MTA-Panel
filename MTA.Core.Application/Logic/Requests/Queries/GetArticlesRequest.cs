using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetArticlesRequest : IRequest<GetArticlesResponse>
    {
        public int Limit { get; init; } = Constants.ArticlesCount;
    }
}