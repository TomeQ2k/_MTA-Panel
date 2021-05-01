using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddReportCommentRequest : IRequest<AddReportCommentResponse>
    {
        public string ReportId { get; init; }
        public string Content { get; init; }
        public bool IsPrivate { get; init; }
    }

    public class AddReportCommentRequestValidator : AbstractValidator<AddReportCommentRequest>
    {
        public AddReportCommentRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
            RuleFor(x => x.Content).NotNull().MaximumLength(Constants.MaximumReportCommentContentLength);
        }
    }
}