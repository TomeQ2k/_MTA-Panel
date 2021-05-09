using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides fetching bans functionality
    /// </summary>
    public class BanController : BaseController
    {
        public BanController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all current user's bans from database
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(GetUserBansResponse), 200)]
        public async Task<IActionResult> GetUserBans([FromQuery] GetUserBansRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched their bans");

            return this.CreateResponse(response);
        }
    }
}