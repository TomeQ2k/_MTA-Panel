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

namespace MTA.API.Controllers.Admin
{
    /// <summary>
    /// <b>[Authorize=Owner]</b> <br/><br/>
    /// Controller which provides email templates management
    /// </summary>
    [Route("api/Admin/EmailTemplate")]
    [Authorize(Policy = Constants.OwnerPolicy)]
    public class EmailTemplateController : BaseController
    {
        public EmailTemplateController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Find email template in application static files: /wwwroot/data/email_templates/
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetEmailTemplateResponse), 200)]
        public async Task<IActionResult> GetEmailTemplate([FromQuery] GetEmailTemplateRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched email template: {request.TemplateName}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Find all email templates in application static files: /wwwroot/data/email_templates/
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(GetEmailTemplatesResponse), 200)]
        public async Task<IActionResult> GetEmailTemplates([FromQuery] GetEmailTemplatesRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched email templates");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Edit email template and save it in application static files: /wwwroot/data/email_templates/
        /// </summary>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(EditEmailTemplateResponse), 200)]
        public async Task<IActionResult> EditEmailTemplate(EditEmailTemplateRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} edited email template: {request.TemplateName}");

            return this.CreateResponse(response);
        }
    }
}