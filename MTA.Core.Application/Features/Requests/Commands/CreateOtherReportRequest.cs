using System.Collections.Generic;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record CreateOtherReportRequest : BaseCreateReportRequest, IRequest<CreateOtherReportResponse>
    {
        public ReportCategoryType Type { get; init; } = ReportCategoryType.Question;
    }

    public class CreateOtherReportRequestValidator : BaseCreateReportRequestValidator<CreateOtherReportRequest>
    {
        private List<ReportCategoryType> conditions = new()
            {ReportCategoryType.Question, ReportCategoryType.Donation, ReportCategoryType.Account};

        public CreateOtherReportRequestValidator()
        {
            RuleFor(x => x.Type).NotNull().IsInEnum().Must(x => conditions.Contains(x));
        }
    }
}