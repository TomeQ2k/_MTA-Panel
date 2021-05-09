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
    /// <b>[Authorize=Owner]</b> <br/><br/>
    /// Controller which provides changelog functionality
    /// </summary>
    [Authorize(Policy = Constants.OwnerPolicy)]
    public class ChangelogController : BaseController
    {
        public ChangelogController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// <b>[AllowAnonymous]</b> <br/><br/>
        /// Get all changelogs from database
        /// </summary>
        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetChangelogsResponse), 200)]
        public async Task<IActionResult> GetChangelogs([FromQuery] GetChangelogsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched changelogs");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create changelog in database
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateChangelogResponse), 200)]
        public async Task<IActionResult> CreateChangelog([FromForm] CreateChangelogRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} created new changelog #{response.Changelog?.Id}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Update changelog in database
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(typeof(UpdateChangelogResponse), 200)]
        public async Task<IActionResult> UpdateChangelog([FromForm] UpdateChangelogRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} updated changelog #{request.ChangelogId}");


            return this.CreateResponse(response);
        }

        /// <summary>
        /// Delete changelog in database
        /// </summary>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(DeleteChangelogResponse), 200)]
        public async Task<IActionResult> DeleteChangelog([FromQuery] DeleteChangelogRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} delete changelog #{request.ChangelogId}");

            return this.CreateResponse(response);
        }
    }
}