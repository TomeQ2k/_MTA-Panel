using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
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