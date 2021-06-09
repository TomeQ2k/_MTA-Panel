using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ChangeUploadedSkinResponse : BaseResponse
    {
        public ChangeUploadedSkinResponse(Error error = null) : base(error)
        {
        }
    }
}