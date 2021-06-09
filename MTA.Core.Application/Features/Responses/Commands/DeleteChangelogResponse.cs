using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record DeleteChangelogResponse : BaseResponse
    {
        public DeleteChangelogResponse(Error error = null) : base(error)
        {
        }
    }
}