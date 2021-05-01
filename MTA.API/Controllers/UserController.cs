using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;

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
        [ProducesResponseType(typeof(GetUsersByUsernameResponse), 200)]
        public async Task<IActionResult> GetUsersByUsername([FromQuery] GetUsersByUsernameRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}