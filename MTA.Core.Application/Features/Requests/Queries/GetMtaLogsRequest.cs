using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetMtaLogsRequest : MtaLogFiltersParams, IRequest<GetMtaLogsResponse>
    {
    }

    public class GetMtaLogsPaginationRequestValidator : AbstractValidator<GetMtaLogsRequest>
    {
        public GetMtaLogsPaginationRequestValidator()
        {
            RuleFor(x => x).Must(x => x.MinTimeAgo - x.MaxTimeAgo <= (int) TimeAgoType.HalfYear) //Todo: Change it!
                .Must(x => x.MinTimeAgo > x.MaxTimeAgo)
                .Must(x => x.SourceAffectedLogType switch
                {
                    SourceAffectedLogType.Account or SourceAffectedLogType.Character => Regex
                        .IsMatch(x.SourceAffected, Constants.UsernameRegex),
                    SourceAffectedLogType.System or SourceAffectedLogType.Pefuel or SourceAffectedLogType.Petoll =>
                        string.IsNullOrEmpty(x.SourceAffected),
                    SourceAffectedLogType.PhoneNumber => Regex.IsMatch(x.SourceAffected, Constants.PhoneNumberRegex),
                    _ => int.TryParse(x.SourceAffected, out _)
                });
            RuleFor(x => x.MinTimeAgo).IsInEnum();
            RuleFor(x => x.Content).MaximumLength(Constants.MaximumLogFilterLength);
            RuleFor(x => x.MaxTimeAgo).IsInEnum();
            RuleFor(x => x.ContentFilterType).IsInEnum();
            RuleFor(x => x.Actions).NotEmpty().Must(x => x.Length <= Constants.LogsActionsMaxCount);
            RuleFor(x => x.SourceAffected).MaximumLength(Constants.MaximumLogFilterLength);
            RuleFor(x => x.SourceAffectedFilterType).IsInEnum();
            RuleFor(x => x.SourceAffectedLogType).IsInEnum();
            RuleFor(x => x.SortType).IsInEnum();
        }
    }
}