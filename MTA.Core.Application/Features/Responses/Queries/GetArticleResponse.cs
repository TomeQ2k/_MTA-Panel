using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetArticleResponse : BaseResponse
    {
        public ArticleDto Article { get; init; }

        public GetArticleResponse(Error error = null) : base(error)
        {
        }
    }
}