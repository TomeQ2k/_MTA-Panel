using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record DeleteArticleResponse : BaseResponse
    {
        public DeleteArticleResponse(Error error = null) : base(error)
        {
        }
    }
}