using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ClearAllNotificationsResponse : BaseResponse
    {
        public ClearAllNotificationsResponse(Error error = null) : base(error)
        {
        }
    }
}