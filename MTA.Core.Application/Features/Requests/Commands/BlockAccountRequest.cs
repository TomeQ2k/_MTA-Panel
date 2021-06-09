using System.Collections.Generic;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record BlockAccountRequest : IRequest<BlockAccountResponse>
    {
        public int AccountId { get; init; }
        public string Reason { get; init; }

        public IEnumerable<string> Serials { get; init; }
        public IEnumerable<string> Ips { get; init; }
    }

    public class BlockAccountRequestValidator : AbstractValidator<BlockAccountRequest>
    {
        public BlockAccountRequestValidator()
        {
            RuleFor(x => x.AccountId).NotNull();
            RuleFor(x => x.Reason).NotNull().MaximumLength(Constants.MaximumBanReasonLength);
        }
    }
}