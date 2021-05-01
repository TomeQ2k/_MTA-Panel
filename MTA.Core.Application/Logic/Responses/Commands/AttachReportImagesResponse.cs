using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AttachReportImagesResponse : BaseResponse
    {
        public AttachReportImagesResponse(Error error = null) : base(error)
        {
        }
    }
}