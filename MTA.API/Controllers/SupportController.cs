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

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides support panel functionality 
    /// </summary>
    public class SupportController : BaseController
    {
        public SupportController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get report from database with relations to: ReportComments, ReportSubscribers, ReportImages
        /// </summary>
        [HttpGet("report/get")]
        [ProducesResponseType(typeof(GetReportResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetReport([FromQuery] GetReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Fetch and filter reports from specified category (or all) from database. Request is paginated
        /// </summary>
        [HttpGet("reports/all")]
        [ProducesResponseType(typeof(FetchAllReportsResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> FetchAllReports([FromQuery] FetchAllReportsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched reports");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// [Authorize=AllOwners]
        /// Fetch and filter archived reports from database. Request is paginated
        /// </summary>
        [HttpGet("reports/archived")]
        [Authorize(Policy = Constants.AllOwnersPolicy)]
        [ProducesResponseType(typeof(FetchArchivedReportsResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> FetchArchivedReports(
            [FromQuery] FetchArchivedReportsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched archived reports");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Fetch and filter current user's reports from database. Request is paginated
        /// </summary>
        [HttpGet("reports/user")]
        [ProducesResponseType(typeof(FetchReportsByUserResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> FetchReportsByUser([FromQuery] FetchReportsByUserRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched their reports");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create report in all categories except: Bug, Penalty and User
        /// </summary>
        [HttpPost("otherReport/create")]
        [ProducesResponseType(typeof(CreateOtherReportResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOtherReport([FromForm] CreateOtherReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} created other report");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create report in bug category
        /// </summary>
        [HttpPost("bugReport/create")]
        [ProducesResponseType(typeof(CreateBugReportResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBugReport([FromForm] CreateBugReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} created bug report");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create report in penalty category
        /// </summary>
        [HttpPost("penaltyReport/create")]
        [ProducesResponseType(typeof(CreatePenaltyReportResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePenaltyReport([FromForm] CreatePenaltyReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} created penalty report");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create report in user category
        /// </summary>
        [HttpPost("userReport/create")]
        [ProducesResponseType(typeof(CreateUserReportResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreateUserReport([FromForm] CreateUserReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} created user report");

            return this.CreateResponse(response);
        }
    }
}