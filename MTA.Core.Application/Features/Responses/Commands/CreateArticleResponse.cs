using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record CreateArticleResponse : BaseResponse
    {
        public ArticleDto Article { get; init; }

        public CreateArticleResponse(Error error = null) : base(error)
        {
        }
    }
}