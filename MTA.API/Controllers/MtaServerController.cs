using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides functionality precisely connected with MTA game server scripts
    /// </summary>
    [Route("api/[controller]/call")]
    public class MtaServerController : BaseController
    {
        public MtaServerController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// <b>[AllowAnonymous]</b> <br/><br/>
        /// Get home page stats using MTA game server script
        /// </summary>    
        [HttpGet("homepageStats")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetHomepageStatsResponse), 200)]
        public async Task<IActionResult> GetHomepageStats([FromQuery] GetHomepageStatsRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}