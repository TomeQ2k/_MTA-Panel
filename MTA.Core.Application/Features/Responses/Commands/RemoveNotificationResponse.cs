using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record RemoveNotificationResponse : BaseResponse
    {
        public RemoveNotificationResponse(Error error = null) : base(error)
        {
        }
    }
}