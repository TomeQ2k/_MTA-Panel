using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record MarkAsReadNotificationResponse : BaseResponse
    {
        public MarkAsReadNotificationResponse(Error error = null) : base(error)
        {
        }
    }
}