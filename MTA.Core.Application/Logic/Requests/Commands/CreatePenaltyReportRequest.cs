using System.Collections.Generic;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record CreatePenaltyReportRequest : BaseCreateReportRequest, IRequest<CreatePenaltyReportResponse>
    {
        public ReportCategoryType Type { get; init; } = ReportCategoryType.Ban;
        public int? BanId { get; init; }
        public int? PenaltyId { get; init; }
    }

    public class CreatePenaltyReportRequestValidator : BaseCreateReportRequestValidator<CreatePenaltyReportRequest>
    {
        private List<ReportCategoryType> conditions = new() {ReportCategoryType.Penalty, ReportCategoryType.Ban};

        public CreatePenaltyReportRequestValidator()
        {
            RuleFor(x => x.Type).NotNull().IsInEnum().Must(x => conditions.Contains(x));
        }
    }
}