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
    public class GetArticlesQuery : IRequestHandler<GetArticlesRequest, GetArticlesResponse>
    {
        private readonly IReadOnlyArticleService articleService;
        private readonly IMapper mapper;

        public GetArticlesQuery(IReadOnlyArticleService articleService, IMapper mapper)
        {
            this.articleService = articleService;
            this.mapper = mapper;
        }

        public async Task<GetArticlesResponse> Handle(GetArticlesRequest request, CancellationToken cancellationToken)
            => new GetArticlesResponse
                {Articles = mapper.Map<IEnumerable<ArticleDto>>(await articleService.GetArticles(request.Limit))};
    }
}