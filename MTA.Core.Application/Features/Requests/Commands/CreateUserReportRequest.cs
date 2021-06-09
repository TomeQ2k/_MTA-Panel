using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record CreateUserReportRequest : BaseCreateReportRequest, IRequest<CreateUserReportResponse>
    {
        public int UserToReportId { get; init; }
        public int? WitnessId { get; init; }
    }

    public class CreateUserReportRequestValidator : BaseCreateReportRequestValidator<CreateUserReportRequest>
    {
        public CreateUserReportRequestValidator()
        {
            RuleFor(x => x.UserToReportId).NotNull();
        }
    }
}