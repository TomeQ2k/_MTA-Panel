using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MTA.Core.Application.Features.Responses;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Validation
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState, Error error)
            : base(new ValidationResponse(modelState, error))
            => (StatusCode) = (StatusCodes.Status422UnprocessableEntity);
    }
}