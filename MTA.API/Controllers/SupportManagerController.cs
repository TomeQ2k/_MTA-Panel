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
    /// Controller which provides support panel management
    /// </summary>
    public class SupportManagerController : BaseController
    {
        public SupportManagerController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// <b>[Authorize=AdminsAndSupporters]</b> <br/><br/>
        /// Find all users who are allowed to be assigned to specified category report 
        /// </summary>
        [HttpGet("allowedAssignees")]
        [Authorize(Policy = Constants.AdminsAndSupportersPolicy)]
        [ProducesResponseType(typeof(GetReportsAllowedAssigneesResponse), 200)]
        public async Task<IActionResult> GetReportsAllowedAssignees(
            [FromQuery] GetReportsAllowedAssigneesRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched reports allowed assignees");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add report comment to specified report
        /// </summary>
        [HttpPost("report/addComment")]
        [ProducesResponseType(typeof(AddReportCommentResponse), 200)]
        public async Task<IActionResult> AddReportComment(AddReportCommentRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added report comment #{response.ReportComment?.Id} to report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add report subscriber to specified report
        /// </summary>
        [HttpPost("report/addSubscriber")]
        [ProducesResponseType(typeof(AddReportSubscriberResponse), 200)]
        public async Task<IActionResult> AddReportSubscriber(AddReportSubscriberRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added report subscriber #{response.ReportSubscriber?.UserId} to report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Remove report subscriber in specified report
        /// </summary>
        [HttpDelete("report/removeSubscriber")]
        [ProducesResponseType(typeof(RemoveReportSubscriberResponse), 200)]
        public async Task<IActionResult> RemoveReportSubscriber([FromQuery] RemoveReportSubscriberRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} removed report subscriber #{request.UserId} from report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Close specified report
        /// </summary>
        [HttpPatch("report/close")]
        [ProducesResponseType(typeof(CloseReportResponse), 200)]
        public async Task<IActionResult> CloseReport(CloseReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} closed report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Archive specified report
        /// </summary>
        [HttpPatch("report/archive")]
        [ProducesResponseType(typeof(ArchiveReportResponse), 200)]
        public async Task<IActionResult> ArchiveReport(ArchiveReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} archived report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Toggle privacy status of specified report
        /// </summary>
        [HttpPatch("report/togglePrivacy")]
        [ProducesResponseType(typeof(TogglePrivacyReportResponse), 200)]
        public async Task<IActionResult> TogglePrivacyReport(TogglePrivacyReportRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} changed report #{request.ReportId} privacy to: {(response.IsPrivate ? "PRIVATE" : "NOT PRIVATE")}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Move specified report assignment to another allowed user
        /// </summary>
        [HttpPatch("report/moveAssignment")]
        [ProducesResponseType(typeof(MoveReportAssignmentResponse), 200)]
        public async Task<IActionResult> MoveReportAssignment(MoveReportAssignmentRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} moved report #{request.ReportId} assignment to user #{request.NewAssigneeId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Accept automatic report assignment
        /// </summary>
        [HttpPatch("report/acceptAssignment")]
        [ProducesResponseType(typeof(AcceptReportAssignmentResponse), 200)]
        public async Task<IActionResult> AcceptReportAssignment(AcceptReportAssignmentRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} accepted assignment to report #{request.ReportId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Reject automatic report assignment and set AssigneeId as null
        /// </summary>
        [HttpPatch("report/rejectAssignment")]
        [ProducesResponseType(typeof(RejectReportAssignmentResponse), 200)]
        public async Task<IActionResult> RejectReportAssignment(RejectReportAssignmentRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} rejected assignment to report #{request.ReportId}");

            return this.CreateResponse(response);
        }
    }
}