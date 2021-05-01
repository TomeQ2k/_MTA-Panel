using FluentValidation;
using MediatR;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record EditEmailTemplateRequest : IRequest<EditEmailTemplateResponse>
    {
        public string TemplateName { get; init; }
        public string Subject { get; init; }
        public string Content { get; init; }
    }

    public class EditEmailTemplateRequestValidator : AbstractValidator<EditEmailTemplateRequest>
    {
        public EditEmailTemplateRequestValidator()
        {
            RuleFor(x => x.TemplateName).NotNull().Must(x =>
                EmailTemplateDictionary.EmailTemplatesDictionary.ContainsKey(x));
            RuleFor(x => x.Subject).NotNull().MaximumLength(Constants.MaximumEmailSubjectLength);
            RuleFor(x => x.Content).NotNull().MaximumLength(Constants.MaximumEmailContentLength);
        }
    }
}