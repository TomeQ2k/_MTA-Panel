using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Helpers;

namespace MTA.Core.Application.Validators
{
    public static class FileExtensionsValidator
    {
        public static IRuleBuilderOptions<T, TElement> AllowedFileExtensionsAre<T, TElement>(
            this IRuleBuilderOptions<T, TElement> ruleBuilder, bool isCollection = false, params string[] extensions)
            => ruleBuilder.Must(upload => isCollection switch
            {
                false when upload != null && upload is IFormFile => IsValidExtension(upload as IFormFile, extensions),
                true when upload != null && upload is List<IFormFile> => (upload as List<IFormFile>)
                    .All(f => IsValidExtension(f, extensions)),
                _ => true
            }).WithMessage(ValidatorMessages.FileExtensionsValidatorMessage(extensions));

        #region private

        private static bool IsValidExtension(IFormFile file, string[] extensions) =>
            extensions.Any(e => e == Path.GetExtension(file.FileName.ToLower()));

        #endregion
    }
}