using System;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
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