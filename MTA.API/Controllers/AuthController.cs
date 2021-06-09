using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Common.Enums;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[AllowAnonymous]</b> <br/><br/>
    /// Controller which provides authentication functionality
    /// </summary>
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Sign in user using email/username and password
        /// </summary>
        [HttpPost("signIn")]
        [ProducesResponseType(typeof(SignInResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User {request.Login} signed in");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Sign up user using username, email, password and MTA serial. Request is secured by Captcha. Request also allow to create account thanks to the reference of another user
        /// </summary>
        [HttpPost("signUp")]
        [ProducesResponseType(typeof(SignUpResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User {request.Email} signed up");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Confirm account (activate) using user's email and security token created during signing up
        /// </summary>
        [HttpGet("confirm")]
        [ProducesResponseType(typeof(ConfirmAccountResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ConfirmAccount([FromQuery] ConfirmAccountRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User {request.Email} confirmed their account");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send activation account email to provided email address
        /// </summary>
        [HttpPost("confirm/send")]
        [ProducesResponseType(typeof(SendActivationEmailResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendActivationEmail(SendActivationEmailRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User {request.Email} resent activation account email link to email address: {request.Email}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Reset user's password directly when the user use reset password link sent to their email address
        /// </summary>
        [HttpPut("resetPassword")]
        [ProducesResponseType(typeof(ResetPasswordResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User {request.Email} resetted their password");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Send reset password email to provided email address. Request is secured by Captcha
        /// </summary>
        [HttpPost("resetPassword/send")]
        [ProducesResponseType(typeof(SendResetPasswordResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> SendResetPassword(SendResetPasswordRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User {request.Login} sent reset password link to their email address");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Verify reset password link and token. Request should be called before ResetPassword
        /// </summary>
        [HttpGet("resetPassword/verify")]
        [ProducesResponseType(typeof(VerifyResetPasswordResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyResetPassword([FromQuery] VerifyResetPasswordRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User {request.Email} verified their reset password link");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Validate username (0) or email address (1) if not already exist in database
        /// </summary>
        [HttpGet("validations")]
        [ProducesResponseType(typeof(GetAuthValidationsResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthValidations([FromQuery] GetAuthValidationsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"{(request.AuthValidationType == AuthValidationType.Email ? "EMAIL" : "USERNAME")} '{request.Login}' availability validated with status: {(response.IsAvailable ? "AVAILABLE" : "NOT AVAILABLE")}");

            return this.CreateResponse(response);
        }
    }
}