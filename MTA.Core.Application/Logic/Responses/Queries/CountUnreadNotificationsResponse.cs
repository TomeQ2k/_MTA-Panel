using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record CountUnreadNotificationsResponse : BaseResponse
    {
        public int UnreadNotificationsCount { get; init; }

        public CountUnreadNotificationsResponse(Error error = null) : base(error)
        {
        }
    }
}