using System.Net;
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
    /// Controller which provides RP test functionality
    /// </summary>
    public class RPTestController : BaseController
    {
        public RPTestController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get specified test part (1 or 2) random questions from database
        /// </summary>
        [HttpGet("partQuestions")]
        [ProducesResponseType(typeof(GetPartQuestionsResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetPartQuestions([FromQuery] GetPartQuestionsRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} fetched questions for RP test part: {(int) request.PartType}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Process current user's test part one answers on the questions and validate if user pass part one
        /// </summary>
        [HttpPut("partOne/pass")]
        [ProducesResponseType(typeof(PassRPTestPartOneResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> PassRPTestPartOne(PassRPTestPartOneRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} processed RP test part one answers on the questions with status: {(response.IsPassed ? "PASSED" : "NOT PASSED")}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Process current user's test part two answers on the questions and generate RP test application in database
        /// </summary>
        [HttpPost("partTwo/generateAnswers")]
        [ProducesResponseType(typeof(GenerateAnswersForPartTwoResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateAnswersForPartTwo(GenerateAnswersForPartTwoRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} generated answers for RP TEST part two");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// [Authorize=AdminsAndSupporters]
        /// Review RP test application and change application state (1 - passed, 2 - not passed). Reviewer has to add note about their review
        /// </summary>
        [HttpPut("partTwo/review")]
        [Authorize(Policy = Constants.AdminsAndSupportersPolicy)]
        [ProducesResponseType(typeof(ReviewRPTestResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> ReviewRPTest(ReviewRPTestRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information(
                $"User #{HttpContext.GetCurrentUserId()} reviewed RP test #{request.ApplicationId} and set state status to: {(int) request.StateType}");

            return this.CreateResponse(response);
        }
    }
}