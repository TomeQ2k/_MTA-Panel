using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Helpers;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetEmailTemplateRequest : IRequest<GetEmailTemplateResponse>
    {
        public string TemplateName { get; init; }
    }

    public class GetEmailTemplateRequestValidator : AbstractValidator<GetEmailTemplateRequest>
    {
        public GetEmailTemplateRequestValidator()
        {
            RuleFor(x => x.TemplateName).NotNull()
                .Must(x => EmailTemplateDictionary.EmailTemplatesDictionary.ContainsKey(x));
        }
    }
}