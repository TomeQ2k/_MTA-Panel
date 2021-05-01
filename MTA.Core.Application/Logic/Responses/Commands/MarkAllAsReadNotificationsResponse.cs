using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record MarkAllAsReadNotificationsResponse : BaseResponse
    {
        public MarkAllAsReadNotificationsResponse(Error error = null) : base(error)
        {
        }
    }
}