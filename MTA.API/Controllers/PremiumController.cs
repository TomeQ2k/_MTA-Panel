using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Factories;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Common.Enums;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides premium panel functionality
    /// </summary>
    public class PremiumController : BaseController
    {
        public PremiumController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Add object protection for estate/vehicle on specified days amount
        /// </summary>
        [HttpPut("objectProtection/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddObjectProtectionCost)]
        [ProducesResponseType(typeof(AddObjectProtectionResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddObjectProtection([FromForm] AddObjectProtectionRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added object protection to: {(request.ProtectionType == ObjectProtectionType.Estate ? "ESTATE" : "VEHICLE")} #{request.ObjectId} for: {request.Amount} days");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add specified amount of serials slots to current account
        /// </summary>
        [HttpPatch("serialSlot/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddSerialSlotCost)]
        [ProducesResponseType(typeof(AddSerialSlotResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddSerialSlot([FromForm] AddSerialSlotRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added {request.Amount} serial slots to their account");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Buy and add custom skin file to current user's premium files library
        /// </summary>
        [HttpPost("customSkin/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddCustomSkinCost)]
        [ProducesResponseType(typeof(AddCustomSkinResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddCustomSkin([FromForm] AddCustomSkinRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added custom skin #{request.SkinId} to their character #{request.CharacterId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Buy and add custom interior file to current user's premium files library
        /// </summary>
        [HttpPost("customInterior/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddCustomInteriorCost)]
        [ProducesResponseType(typeof(AddCustomInteriorResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddCustomInterior([FromForm] AddCustomInteriorRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} added custom interior to their estate #{request.EstateId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Transfer character and their relations: Estates, Vehicles, GameItems to another current user's character
        /// </summary>
        [HttpPut("transferCharacter")]
        [PremiumFilterFactory(Cost = PremiumConstants.TransferCharacterCost)]
        [ProducesResponseType(typeof(TransferCharacterResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> TransferCharacter([FromForm] TransferCharacterRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} transfered their character #{request.SourceCharacterId} to character #{request.TargetCharacterId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Restore default skin for specified character using MTA game server script
        /// </summary>
        [HttpPost("restoreDefaultSkin")]
        [ProducesResponseType(typeof(RestoreDefaultSkinResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> RestoreDefaultSkin(RestoreDefaultSkinRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} restored default skin for character #{request.CharacterId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Restore default interior for specified estate using MTA game server script
        /// </summary>
        [HttpPost("restoreDefaultInterior")]
        [ProducesResponseType(typeof(RestoreDefaultInteriorResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> RestoreDefaultInterior(RestoreDefaultInteriorRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} restored default interior for estate #{request.EstateId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Donate server for specified price. Premium credits will be added to current user's account automatically in the next 24 hours
        /// </summary>
        [HttpPost("donateServer")]
        [ProducesResponseType(typeof(DonateServerResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> DonateServer(DonateServerRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} donated server for: {(int) request.DonationType} PLN");

            return this.CreateResponse(response);
        }
    }
}