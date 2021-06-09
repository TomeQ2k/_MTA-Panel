using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class TransferCharacterCommand : IRequestHandler<TransferCharacterRequest, TransferCharacterResponse>
    {
        private readonly IPremiumAccountManager premiumAccountManager;
        private readonly IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint;
        private readonly IHttpContextReader httpContextReader;

        public TransferCharacterCommand(IPremiumAccountManager premiumAccountManager,
            IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint,
            IHttpContextReader httpContextReader)
        {
            this.premiumAccountManager = premiumAccountManager;
            this.premiumCreditsDatabaseRestorePoint = premiumCreditsDatabaseRestorePoint;
            this.httpContextReader = httpContextReader;
        }

        public async Task<TransferCharacterResponse> Handle(TransferCharacterRequest request,
            CancellationToken cancellationToken)
        {
            using (var _ = premiumCreditsDatabaseRestorePoint.CreateRestoreParams(
                new PremiumCreditsDatabaseRestoreParams(PremiumConstants.TransferCharacterCost,
                    httpContextReader.CurrentUserId)))
            {
                return await premiumAccountManager.TransferCharacter(request.SourceCharacterId,
                    request.TargetCharacterId)
                    ? (TransferCharacterResponse) new TransferCharacterResponse()
                        .LogInformation($"user #{httpContextReader.CurrentUserId} " +
                                        $"has transferred items from character #{request.SourceCharacterId} " +
                                        $"to character #{request.TargetCharacterId}")
                    : throw new PremiumOperationException(
                        $"Premium operation of transfering character from character #{request.SourceCharacterId} to character #{request.TargetCharacterId} failed");
            }
        }
    }
}