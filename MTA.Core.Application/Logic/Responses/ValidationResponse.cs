using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses
{
    public record ValidationResponse : BaseResponse
    {
        public List<ValidationError> ValidationErrors { get; init; }

        public ValidationResponse(ModelStateDictionary modelState, Error error = null)
            : base(error)
            => (ValidationErrors) = (modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(e => new ValidationError(key, e.ErrorMessage)))
                .ToList());
    }
}