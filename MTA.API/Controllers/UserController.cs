using System.Net;
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
    /// Controller which provides fetching users functionality
    /// </summary>
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get limited (50) users from database by specified username
        /// </summary>
        [HttpGet("all/byUsername")]
        [ProducesResponseType(typeof(GetUsersByUsernameResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersByUsername([FromQuery] GetUsersByUsernameRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched users by username");

            return this.CreateResponse(response);
        }
    }
}