using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public abstract record BaseCreateReportRequest
    {
        public DateTime? EventDate { get; init; }
        public string Subject { get; init; }
        public string Content { get; init; }
        public bool IsPrivate { get; init; }

        public ICollection<IFormFile> Images { get; init; }
    }

    public class BaseCreateReportRequestValidator<T> : AbstractValidator<T> where T : BaseCreateReportRequest
    {
        public BaseCreateReportRequestValidator()
        {
            RuleFor(x => x.Subject).MaximumLength(Constants.MaximumReportSubjectLength);
            RuleFor(x => x.Content).NotNull().MaximumLength(Constants.MaximumReportContentLength);
            RuleForEach(x => x.Images).NotNull().AllowedFileExtensionsAre(isCollection: false, ".img", ".png", ".jpg",
                ".jpeg", ".tiff", ".ico", ".svg");
        }
    }
}