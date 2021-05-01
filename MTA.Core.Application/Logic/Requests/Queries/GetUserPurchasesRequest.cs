using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetUserPurchasesRequest : BasePurchaseFiltersParams, IRequest<GetUserPurchasesResponse>
    {
    }

    public class GetUserPurchasesRequestValidator : AbstractValidator<GetUserPurchasesRequest>
    {
        public GetUserPurchasesRequestValidator()
        {
            RuleFor(x => x.SortType).IsInEnum();
        }
    }
}