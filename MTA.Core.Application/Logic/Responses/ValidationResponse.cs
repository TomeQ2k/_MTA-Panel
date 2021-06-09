using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses
{
    public record ValidationResponse : BaseResponse
    {
        public IDictionary<string, ValidationError> ValidationErrors { get; }

        public ValidationResponse(ModelStateDictionary modelState, Error error = null)
            : base(error)
            => (ValidationErrors) = (modelState.Keys
                .GroupBy(key => key, key => modelState[key].Errors.Select(e => e.ErrorMessage))
                .ToDictionary(g => g.Key, g => new ValidationError(g.Key, g.First().ToList())));
    }
}