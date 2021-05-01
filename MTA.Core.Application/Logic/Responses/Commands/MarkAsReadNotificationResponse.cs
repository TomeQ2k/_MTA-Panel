using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record MarkAsReadNotificationResponse : BaseResponse
    {
        public MarkAsReadNotificationResponse(Error error = null) : base(error)
        {
        }
    }
}