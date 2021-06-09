using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class CreateArticleCommand : IRequestHandler<CreateArticleRequest, CreateArticleResponse>
    {
        private readonly IArticleService articleService;
        private readonly IMapper mapper;

        public CreateArticleCommand(IArticleService articleService, IMapper mapper)
        {
            this.articleService = articleService;
            this.mapper = mapper;
        }

        public async Task<CreateArticleResponse> Handle(CreateArticleRequest request,
            CancellationToken cancellationToken)
        {
            var article = await articleService.CreateArticle(request) ??
                                 throw new CrudException("Creating article failed");

            return new CreateArticleResponse {Article = mapper.Map<ArticleDto>(article)};
        }
    }
}