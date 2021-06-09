using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record UpdateArticleResponse : BaseResponse
    {
        public ArticleDto Article { get; init; }

        public UpdateArticleResponse(Error error = null) : base(error)
        {
        }
    }
}