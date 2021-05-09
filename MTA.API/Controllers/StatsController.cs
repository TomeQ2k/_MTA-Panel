using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[AllowAnonymous]</b> <br/><br/>
    /// Controller which provides fetching stats functionality
    /// </summary>
    [AllowAnonymous]
    public class StatsController : BaseController
    {
        public StatsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Fetch game stats from file: /wwwroot/data/stats.json or local memory cache
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FetchStatsResponse), 200)]
        public async Task<IActionResult> FetchStats([FromQuery] FetchStatsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} fetched server stats");

            return this.CreateResponse(response);
        }
    }
}