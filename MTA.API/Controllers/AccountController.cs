using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Features.Responses.Queries;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides user's account functionality
    /// </summary>
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get user's account data from database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetCurrentUserResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentUser([FromQuery] GetCurrentUserRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} displayed profile");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Change user's password when user use their change password link (token) received on provided before email address (SendChangePasswordEmail)
        /// </summary>
        [HttpGet("changePassword")]
        [ProducesResponseType(typeof(ChangePasswordResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword([FromQuery] ChangePasswordRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} changed their password");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Change user's email when user use their change email link (token) received on provided before email address (SendChangeEmailEmail)
        /// </summary>
        [HttpGet("changeEmail")]
        [ProducesResponseType(typeof(ChangeEmailResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeEmail([FromQuery] ChangeEmailRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} changed their email address to: {request.NewEmail}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add new MTA serial to user's account when user use their add serial link (token) received on provided before email address (SendAddSerialEmail)
        /// </summary>
        [HttpGet("serial/add")]
        [ProducesResponseType(typeof(AddSerialResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddSerial([FromQuery] AddSerialRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added serial to their account: {request.Serial}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send change password token to provided email address
        /// </summary>
        [HttpPost("changePassword/send")]
        [ProducesResponseType(typeof(SendChangePasswordEmailResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendChangePasswordEmail(SendChangePasswordEmailRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} sent change password link to their email address");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send change email token to provided email address
        /// </summary>
        [HttpPost("changeEmail/send")]
        [ProducesResponseType(typeof(SendChangeEmailEmailResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendChangeEmailEmail(SendChangeEmailEmailRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} sent change email link to email address: {request.NewEmail}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send add serial token to provided email address
        /// </summary>
        [HttpPost("serial/add/send")]
        [ProducesResponseType(typeof(SendAddSerialEmailResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendAddSerialEmail(SendAddSerialEmailRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} sent add serial link to their email address");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Delete one user's MTA serial using serial id
        /// </summary>
        [HttpDelete("serial/delete")]
        [ProducesResponseType(typeof(DeleteSerialResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteSerial([FromQuery] DeleteSerialRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} deleted serial from their account: #{request.SerialId}");

            return this.CreateResponse(response);
        }
    }
}