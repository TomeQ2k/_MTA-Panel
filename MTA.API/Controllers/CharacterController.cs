using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides fetching characters functionality
    /// </summary>
    public class CharacterController : BaseController
    {
        public CharacterController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get limited (50) characters from database by specified character name
        /// </summary>
        [HttpGet("all/byCharactername")]
        [ProducesResponseType(typeof(GetCharactersByCharacternameResponse), 200)]
        public async Task<IActionResult> GetCharactersByCharactername(
            [FromQuery] GetCharactersByCharacternameRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Get limited (50) characters from database by specified character name with relation to User
        /// </summary>
        [HttpGet("all/withUserByCharactername")]
        [ProducesResponseType(typeof(GetCharactersWithUserByCharacternameResponse), 200)]
        public async Task<IActionResult> GetCharactersWithUserByCharactername(
            [FromQuery] GetCharactersWithUserByCharacternameRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}