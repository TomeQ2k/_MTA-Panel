using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Factories;
using MTA.Core.Application.Filters;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;

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
        [ProducesResponseType(typeof(AddObjectProtectionResponse), 200)]
        public async Task<IActionResult> AddObjectProtection([FromForm] AddObjectProtectionRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Add specified amount of serials slots to current account
        /// </summary>
        [HttpPatch("serialSlot/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddSerialSlotCost)]
        [ProducesResponseType(typeof(AddSerialSlotResponse), 200)]
        public async Task<IActionResult> AddSerialSlot([FromForm] AddSerialSlotRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Buy and add custom skin file to current user's premium files library
        /// </summary>
        [HttpPost("customSkin/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddCustomSkinCost)]
        [ProducesResponseType(typeof(AddCustomSkinResponse), 200)]
        public async Task<IActionResult> AddCustomSkin([FromForm] AddCustomSkinRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Buy and add custom interior file to current user's premium files library
        /// </summary>
        [HttpPost("customInterior/add")]
        [PremiumFilterFactory(Cost = PremiumConstants.AddCustomInteriorCost)]
        [ProducesResponseType(typeof(AddCustomInteriorResponse), 200)]
        public async Task<IActionResult> AddCustomInterior([FromForm] AddCustomInteriorRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Transfer character and their relations: Estates, Vehicles, GameItems to another current user's character
        /// </summary>
        [HttpPut("transferCharacter")]
        [PremiumFilterFactory(Cost = PremiumConstants.TransferCharacterCost)]
        [ProducesResponseType(typeof(TransferCharacterResponse), 200)]
        public async Task<IActionResult> TransferCharacter([FromForm] TransferCharacterRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Restore default skin for specified character using MTA game server script
        /// </summary>
        [HttpPost("restoreDefaultSkin")]
        [ProducesResponseType(typeof(RestoreDefaultSkinResponse), 200)]
        public async Task<IActionResult> RestoreDefaultSkin(RestoreDefaultInteriorRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Restore default interior for specified estate using MTA game server script
        /// </summary>
        [HttpPost("restoreDefaultInterior")]
        [ProducesResponseType(typeof(RestoreDefaultInteriorResponse), 200)]
        public async Task<IActionResult> RestoreDefaultInterior(RestoreDefaultInteriorRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Donate server for specified price. Premium credits will be added to current user's account automatically in the next 24 hours
        /// </summary>
        [HttpPost("donateServer")]
        [ProducesResponseType(typeof(DonateServerResponse), 200)]
        public async Task<IActionResult> DonateServer(DonateServerRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}