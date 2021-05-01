using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MTA.Core.Application.Logic.Responses;

namespace MTA.Core.Application.Models
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState, Error error)
            : base(new ValidationResponse(modelState, error))
            => (StatusCode) = (StatusCodes.Status422UnprocessableEntity);
    }
}