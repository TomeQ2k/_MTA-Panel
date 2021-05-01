using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record DeleteArticleResponse : BaseResponse
    {
        public DeleteArticleResponse(Error error = null) : base(error)
        {
        }
    }
}