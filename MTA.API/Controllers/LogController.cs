using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize=Team]</b> <br/><br/>
    /// Controller which provides fetching logs functionality
    /// </summary>
    [Authorize(Policy = Constants.TeamPolicy)]
    public class LogController : BaseController
    {
        public LogController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get and filter MTA logs from database. Request is paginated
        /// </summary>
        [HttpPost("mtaLogs/filter")]
        [ProducesResponseType(typeof(GetMtaLogsResponse), 200)]
        public async Task<IActionResult> GetMtaLogs(GetMtaLogsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched MTA logs");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// <b>[Authorize=Owner]</b> <br/><br/>
        /// Get and filter web API logs from log file: /wwwroot/logs/ for specified date. Request is paginated
        /// </summary>
        [HttpGet("apiLogs/filter")]
        [Authorize(Policy = Constants.OwnerPolicy)]
        [ProducesResponseType(typeof(GetApiLogsResponse), 200)]
        public async Task<IActionResult> GetApiLogs([FromQuery] GetApiLogsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched API logs");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get current user's allowed log actions due to their log read permissions
        /// </summary>
        [HttpGet("allowedActions")]
        [ProducesResponseType(typeof(GetAllowedLogActionsResponse), 200)]
        public async Task<IActionResult> GetAllowedLogActions([FromQuery] GetAllowedLogActionsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched their allowed log actions");

            return this.CreateResponse(response);
        }
    }
}