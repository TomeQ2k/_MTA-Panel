using FluentValidation;
using MediatR;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
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