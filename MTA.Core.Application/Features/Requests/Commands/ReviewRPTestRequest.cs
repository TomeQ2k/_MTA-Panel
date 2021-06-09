using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record ReviewRPTestRequest : IRequest<ReviewRPTestResponse>
    {
        public int ApplicationId { get; init; }
        public string Note { get; init; }
        public ApplicationStateType StateType { get; init; }
    }

    public class ReviewRPTestRequestValidator : AbstractValidator<ReviewRPTestRequest>
    {
        public ReviewRPTestRequestValidator()
        {
            RuleFor(x => x.ApplicationId).NotNull();
            RuleFor(x => x.Note).NotNull().MaximumLength(Constants.MaximumReviewRPTestNote);
            RuleFor(x => x.StateType).NotNull().IsInEnum();
        }
    }
}