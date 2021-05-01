using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record CreateBugReportRequest : BaseCreateReportRequest, IRequest<CreateBugReportResponse>
    {
        public ReportBugType BugType { get; init; }
        public string AdditionalInfo { get; init; }
    }

    public class CreateBugReportRequestValidator : BaseCreateReportRequestValidator<CreateBugReportRequest>
    {
        public CreateBugReportRequestValidator()
        {
            RuleFor(x => x.BugType).NotNull().IsInEnum();
            RuleFor(x => x.AdditionalInfo).MaximumLength(Constants.MaximumBugReportAdditionalInfoLength);
        }
    }
}