using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.Controllers.Admin
{
    /// <summary>
    /// <b>[Authorize=AllAdmins]</b> <br/><br/>
    /// Controller which provides user management system
    /// </summary>
    [Route("api/Admin/User")]
    [Authorize(Policy = Constants.AllAdminsPolicy)]
    public class AdminUserController : BaseController
    {
        public AdminUserController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get user from database with relations to: Characters (Estates, Vehicles) and Serials
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetUserWithCharactersResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserByAdmin([FromQuery] GetUserWithCharactersRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched user #{request.UserId} as admin");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get and filter users from database. Request is paginated
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(GetUsersByAdminResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersByAdmin([FromQuery] GetUsersByAdminRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched users as admin");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send reset password email to provided user's email address (find user by username/email)
        /// </summary>
        [HttpPost("resetPassword/send")]
        [ProducesResponseType(typeof(SendResetPasswordByAdminResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendResetPasswordByAdmin(SendResetPasswordByAdminRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} sent reset password link to user: {request.Login}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send change email address email to provided user's new email address (find user by id)
        /// </summary>
        [HttpPost("changeEmail/send")]
        [ProducesResponseType(typeof(SendChangeEmailEmailByAdminResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendChangeEmailEmailByAdmin(SendChangeEmailEmailByAdminRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} sent change email link to user's email address: {request.NewEmail}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get all MTA serials specified for provided user
        /// </summary>
        [HttpGet("serials")]
        [ProducesResponseType(typeof(GetUserSerialsResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserSerials([FromQuery] GetUserSerialsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched user #{request.UserId} serials");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// [Authorize=AllOwners]
        /// Block user's account (their serials and IPs) providing account id and block reason
        /// </summary>
        [HttpPost("block")]
        [ProducesResponseType(typeof(BlockAccountResponse), (int) HttpStatusCode.OK)]
        [Authorize(Policy = Constants.AllOwnersPolicy)]
        public async Task<IActionResult> BlockAccount(BlockAccountRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} blocked account #{request.AccountId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add provided credits amount to specified user
        /// </summary>
        [HttpPatch("credits/add")]
        [ProducesResponseType(typeof(AddCreditsResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddCredits(AddCreditsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added {request.Credits} credits to account #{request.UserId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// [Authorize=AllOwners]
        /// Clean account for specified user. Request deletes from database all user's Characters (Estates, Vehicles, GameItems)
        /// </summary>
        [HttpDelete("clean")]
        [Authorize(Policy = Constants.AllOwnersPolicy)]
        [ProducesResponseType(typeof(CleanAccountResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CleanAccount([FromQuery] CleanAccountRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} cleaned account #{request.UserId}");

            return this.CreateResponse(response);
        }
    }
}