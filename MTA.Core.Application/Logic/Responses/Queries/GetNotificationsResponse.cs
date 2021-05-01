using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetNotificationsResponse : BaseResponse
    {
        public IEnumerable<NotificationDto> Notifications { get; init; }

        public GetNotificationsResponse(Error error = null) : base(error)
        {
        }
    }
}