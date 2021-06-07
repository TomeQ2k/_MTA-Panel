using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides user's premium files library functionality: /wwwroot/files/premium/library/
    /// </summary>
    public class PremiumUserLibraryController : BaseController
    {
        public PremiumUserLibraryController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Fetch all current user's premium files library
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FetchPremiumLibraryFilesResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> FetchLibraryFiles([FromQuery] FetchPremiumLibraryFilesRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched their premium files library");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Change uploaded premium skin file
        /// </summary>
        [HttpPost("skin/change")]
        [ProducesResponseType(typeof(ChangeUploadedSkinResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeUploadedSkin([FromForm] ChangeUploadedSkinRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} changed uploaded premium skin file #{request.OldFileId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Change uploaded premium interior file
        /// </summary>
        [HttpPost("interior/change")]
        [ProducesResponseType(typeof(ChangeUploadedInteriorResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeUploadedInteriorFile([FromForm] ChangeUploadedInteriorRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} changed uploaded premium interior file #{request.OldFileId}");

            return this.CreateResponse(response);
        }
    }
}