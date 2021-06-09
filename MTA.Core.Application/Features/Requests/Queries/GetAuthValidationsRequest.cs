using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetAuthValidationsRequest : IRequest<GetAuthValidationsResponse>
    {
        public string Login { get; init; }
        public AuthValidationType AuthValidationType { get; init; }
    }

    public class GetAuthValidationRequestValidator : AbstractValidator<GetAuthValidationsRequest>
    {
        public GetAuthValidationRequestValidator()
        {
            RuleFor(x => x.AuthValidationType).IsInEnum();
        }
    }
}