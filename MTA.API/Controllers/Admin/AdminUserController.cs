using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Common.Helpers;

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
        public async Task<IActionResult> GetUserByAdmin([FromQuery] GetUserWithCharactersRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get and filter users from database. Request is paginated
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetUsersByAdmin([FromQuery] GetUsersByAdminRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send reset password email to provided user's email address (find user by username/email)
        /// </summary>
        [HttpPost("resetPassword/send")]
        public async Task<IActionResult> SendResetPasswordByAdmin(SendResetPasswordByAdminRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send change email address email to provided user's new email address (find user by id)
        /// </summary>
        [HttpPost("changeEmail/send")]
        public async Task<IActionResult> SendChangeEmailEmailByAdmin(SendChangeEmailEmailByAdminRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get all MTA serials specified for provided user
        /// </summary>
        [HttpGet("serials")]
        public async Task<IActionResult> GetUserSerials([FromQuery] GetUserSerialsRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// <b>[Authorize=AllOwners]</b> <br/><br/>
        /// Block user's account (their serials and IPs) providing account id and block reason
        /// </summary>
        [HttpPost("block")]
        [Authorize(Policy = Constants.AllOwnersPolicy)]
        public async Task<IActionResult> BlockAccount(BlockAccountRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add provided credits amount to specified user
        /// </summary>
        [HttpPatch("credits/add")]
        public async Task<IActionResult> AddCredits(AddCreditsRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// <b>[Authorize=AllOwners]</b> <br/><br/>
        /// Clean account for specified user. Request deletes from database all user's Characters (Estates, Vehicles, GameItems)
        /// </summary>
        [HttpDelete("clean")]
        [Authorize(Policy = Constants.AllOwnersPolicy)]
        public async Task<IActionResult> CleanAccount([FromQuery] CleanAccountRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}