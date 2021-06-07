using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.Controllers.Admin
{
    /// <summary>
    /// <b>[Authorize=AllAdmins]</b> <br/><br/>
    /// Controller which provides character management system
    /// </summary>
    [Route("api/Admin/Character")]
    [Authorize(Policy = Constants.AllAdminsPolicy)]
    public class AdminCharacterController : BaseController
    {
        public AdminCharacterController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get character from database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetCharacterResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCharacterByAdmin([FromQuery] GetCharacterRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} fetched character #{request.CharacterId} as admin");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get and filter characters from database. Request is paginated
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(GetCharactersByAdminResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCharactersByAdmin([FromQuery] GetCharactersByAdminRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched characters as admin");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Toggle block character status
        /// </summary>
        [HttpPatch("block/toggle")]
        [ProducesResponseType(typeof(ToggleBlockCharacterResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ToggleBlockCharacter(ToggleBlockCharacterRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} {(response.IsBlocked ? "blocked" : "unblocked")} character #{request.CharacterId}");

            return this.CreateResponse(response);
        }
    }
}