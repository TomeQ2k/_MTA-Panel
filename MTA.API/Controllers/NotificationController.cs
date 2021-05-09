using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides notification system functionality
    /// </summary>
    public class NotificationController : BaseController
    {
        public NotificationController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all current user's notifications
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(GetNotificationsResponse), 200)]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched their notifications");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Mark specified current user's notification as read
        /// </summary>
        [HttpPatch("read")]
        [ProducesResponseType(typeof(MarkAsReadNotificationResponse), 200)]
        public async Task<IActionResult> MarkAsRead(MarkAsReadNotificationRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} marked notification #{request.NotificationId} as read");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Mark all current user's notifications as read
        /// </summary>
        [HttpPut("readAll")]
        [ProducesResponseType(typeof(MarkAllAsReadNotificationsResponse), 200)]
        public async Task<IActionResult> MarkAllAsRead(MarkAllAsReadNotificationsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} marked all their notifications as read");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Remove specified current user's notification
        /// </summary>
        [HttpDelete("remove")]
        [ProducesResponseType(typeof(RemoveNotificationResponse), 200)]
        public async Task<IActionResult> RemoveNotification([FromQuery] RemoveNotificationRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} removed notification #{request.NotificationId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Clear all current user's notifications
        /// </summary>
        [HttpDelete("clearAll")]
        [ProducesResponseType(typeof(ClearAllNotificationsResponse), 200)]
        public async Task<IActionResult> ClearAllNotifications([FromQuery] ClearAllNotificationsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} cleared all their notifications");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Count all current user's unread notifications
        /// </summary>
        [HttpGet("unread/count")]
        [ProducesResponseType(typeof(CountUnreadNotificationsResponse), 200)]
        public async Task<IActionResult> CountUnreadNotifications([FromQuery] CountUnreadNotificationsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} counted their unread notifications");

            return this.CreateResponse(response);
        }
    }
}