using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class UpdateArticleCommand : IRequestHandler<UpdateArticleRequest, UpdateArticleResponse>
    {
        private readonly IArticleService articleService;
        private readonly IMapper mapper;

        public UpdateArticleCommand(IArticleService articleService, IMapper mapper)
        {
            this.articleService = articleService;
            this.mapper = mapper;
        }

        public async Task<UpdateArticleResponse> Handle(UpdateArticleRequest request,
            CancellationToken cancellationToken)
        {
            var article = await articleService.UpdateArticle(request);

            return article != null
                ? new UpdateArticleResponse {Article = mapper.Map<ArticleDto>(article)}
                : throw new CrudException("Updating article failed");
        }
    }
}