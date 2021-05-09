using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize]</b> <br/><br/>
    /// Controller which provides payment functionality. External payment API is: PayPal
    /// </summary>
    public class PaymentController : BaseController
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Create payment (OrderTransaction) in database and call external payment API. Request returns links which provide payment processing on the external payment API side
        /// </summary>
        [HttpGet("create")]
        [ProducesResponseType(typeof(CreatePaymentResponse), 200)]
        public async Task<IActionResult> CreatePayment([FromQuery] CreatePaymentRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} created payment #{response.Result.OrderId} for {request.Price}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Capture payment using external payment API if transaction succeeded. Change validated status of OrderTransaction in database
        /// </summary>
        [HttpGet("capture")]
        [ProducesResponseType(typeof(CapturePaymentResponse), 200)]
        public async Task<IActionResult> CapturePayment([FromQuery] CapturePaymentRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} captured payment with token: {request.Token} and verified with status: {(response.IsVerified ? "VERIFIED" : "NOT VERIFIED")}");

            return this.CreateResponse(response);
        }
    }
}