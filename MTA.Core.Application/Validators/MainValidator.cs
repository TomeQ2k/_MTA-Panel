using Microsoft.AspNetCore.Mvc.Filters;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Validators
{
    public class MainValidator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new ValidationFailedResult(context.ModelState,
                    Error.Build(ErrorCodes.ValidationError, ValidatorMessages.MainValidatorMessage));
        }
    }
}