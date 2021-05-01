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

namespace MTA.API.Controllers.Admin
{
    /// <summary>
    /// <b>[Authorize=AllAdmins]</b> <br/><br/>
    /// Controller which provides character management system
    /// </summary>
    [Route("api/Admin/Character")]
    [Authorize(Policy = Constants.AllAdminsPolicy)]
    public class AdminCharacterController : BaseController
    {
        public AdminCharacterController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get character from database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetCharacterResponse), 200)]
        public async Task<IActionResult> GetCharacterByAdmin([FromQuery] GetCharacterRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get and filter characters from database. Request is paginated
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(GetCharactersByAdminResponse), 200)]
        public async Task<IActionResult> GetCharactersByAdmin([FromQuery] GetCharactersByAdminRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Toggle block character status
        /// </summary>
        [HttpPatch("block/toggle")]
        [ProducesResponseType(typeof(ToggleBlockCharacterResponse), 200)]
        public async Task<IActionResult> ToggleBlockCharacter(ToggleBlockCharacterRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}