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
        [ProducesResponseType(typeof(GetAdminActionsByActionAndUserIdResponse), 200)]
        public async Task<IActionResult> GetAdminActionsByActionAndUserId(
            [FromQuery] GetAdminActionsByActionAndUserIdRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}