using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides fetching admin actions (adminhistory) from database
    /// </summary>
    public class AdminActionController : BaseController
    {
        public AdminActionController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all admin actions by specified action type and connected to specified user
        /// </summary>
        [HttpGet("byActionAndUserId/filter")]
        [ProducesResponseType(typeof(GetAdminActionsByActionAndUserIdResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAdminActionsByActionAndUserId(
            [FromQuery] GetAdminActionsByActionAndUserIdRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched their admin actions");

            return this.CreateResponse(response);
        }
    }
}