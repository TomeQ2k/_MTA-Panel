using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetCharactersWithUserByCharacternameRequest : IRequest<GetCharactersWithUserByCharacternameResponse>
    {
        public string Charactername { get; init; }
    }

    public class GetReportDataRequestValidator : AbstractValidator<GetCharactersWithUserByCharacternameRequest>
    {
        public GetReportDataRequestValidator()
        {
            RuleFor(x => x.Charactername).NotNull();
        }
    }
}