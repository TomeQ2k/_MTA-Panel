using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Common.Helpers;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides fetching purchases (don_purchases) functionality
    /// </summary>
    public class PurchaseController : BaseController
    {
        public PurchaseController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get current user's purchases from database. Request is paginated
        /// </summary>
        [HttpGet("user/filter")]
        [ProducesResponseType(typeof(GetUserPurchasesResponse), 200)]
        public async Task<IActionResult> GetUserPurchases([FromQuery] GetUserPurchasesRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// <b>[Authorize=AllAdmins]</b> <br/><br/>
        /// Get and filter purchases from database. Request is paginated
        /// </summary>
        [HttpGet("admin/filter")]
        [Authorize(Policy = Constants.AllAdminsPolicy)]
        [ProducesResponseType(typeof(GetPurchasesByAdminResponse), 200)]
        public async Task<IActionResult> GetPurchasesByAdmin([FromQuery] GetPurchasesByAdminRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}