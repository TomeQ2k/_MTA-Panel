using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
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