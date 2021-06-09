using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Validation
{
    public class ValidatorBehavior : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new ValidationFailedResult(context.ModelState,
                    Error.Build(ErrorCodes.ValidationError, ValidatorMessages.DefaultValidatorMessage,
                        HttpStatusCode.UnprocessableEntity));
        }
    }
}