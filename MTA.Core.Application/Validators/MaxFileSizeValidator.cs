using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Helpers;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Validators
{
    public static class MaxFileSizeValidator
    {
        public static IRuleBuilderOptions<T, TElement> MaxFileSizeIs<T, TElement>(
            this IRuleBuilderOptions<T, TElement> ruleBuilder, int maxFileSize, bool isCollection = false)
            => ruleBuilder.Must(upload => isCollection switch
            {
                false when upload != null && upload is IFormFile => (upload as IFormFile).Length <=
                                                                    maxFileSize,
                true when upload != null && upload is List<IFormFile> => (upload as List<IFormFile>)
                    .All(f => f.Length <= maxFileSize),
                _ => true
            }).WithMessage(ValidatorMessages.MaxFileSizeValidatorMessage(maxFileSize));
    }
}