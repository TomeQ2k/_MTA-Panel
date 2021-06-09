using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetArticleQuery : IRequestHandler<GetArticleRequest, GetArticleResponse>
    {
        private readonly IReadOnlyArticleService articleService;
        private readonly IMapper mapper;

        public GetArticleQuery(IReadOnlyArticleService articleService, IMapper mapper)
        {
            this.articleService = articleService;
            this.mapper = mapper;
        }

        public async Task<GetArticleResponse> Handle(GetArticleRequest request, CancellationToken cancellationToken)
            => new GetArticleResponse
                {Article = mapper.Map<ArticleDto>(await articleService.GetArticle(request.ArticleId))};
    }
}