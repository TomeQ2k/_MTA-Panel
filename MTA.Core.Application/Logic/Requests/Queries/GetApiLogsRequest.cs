using System;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetApiLogsRequest : ApiLogFiltersParams, IRequest<GetApiLogsResponse>
    {
    }

    public class GetApiLogsRequestValidator : AbstractValidator<GetApiLogsRequest>
    {
        public GetApiLogsRequestValidator()
        {
            RuleFor(x => x.Date).NotNull().Must(x => x <= DateTime.Now.AddDays(-1));
        }
    }
}