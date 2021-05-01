using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record ChangeUploadedSkinResponse : BaseResponse
    {
        public ChangeUploadedSkinResponse(Error error = null) : base(error)
        {
        }
    }
}