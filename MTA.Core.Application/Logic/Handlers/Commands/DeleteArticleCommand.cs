using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class DeleteArticleCommand : IRequestHandler<DeleteArticleRequest, DeleteArticleResponse>
    {
        private readonly IArticleService articleService;

        public DeleteArticleCommand(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public async Task<DeleteArticleResponse> Handle(DeleteArticleRequest request,
            CancellationToken cancellationToken)
            => await articleService.DeleteArticle(request.ArticleId)
                ? new DeleteArticleResponse()
                : throw new CrudException("Deleting article failed");
    }
}