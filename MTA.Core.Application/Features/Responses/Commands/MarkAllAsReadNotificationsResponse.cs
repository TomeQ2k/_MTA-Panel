using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record MarkAllAsReadNotificationsResponse : BaseResponse
    {
        public MarkAllAsReadNotificationsResponse(Error error = null) : base(error)
        {
        }
    }
}