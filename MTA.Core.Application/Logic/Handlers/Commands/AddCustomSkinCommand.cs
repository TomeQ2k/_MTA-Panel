using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class AddCustomSkinCommand : IRequestHandler<AddCustomSkinRequest, AddCustomSkinResponse>
    {
        private readonly IPremiumAccountManager premiumAccountManager;
        private readonly IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint;
        private readonly IHttpContextReader httpContextReader;

        public AddCustomSkinCommand(IPremiumAccountManager premiumAccountManager,
            IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint,
            IHttpContextReader httpContextReader)
        {
            this.premiumAccountManager = premiumAccountManager;
            this.premiumCreditsDatabaseRestorePoint = premiumCreditsDatabaseRestorePoint;
            this.httpContextReader = httpContextReader;
        }

        public async Task<AddCustomSkinResponse> Handle(AddCustomSkinRequest request,
            CancellationToken cancellationToken)
        {
            using (var _ = premiumCreditsDatabaseRestorePoint.CreateRestoreParams(
                new PremiumCreditsDatabaseRestoreParams(PremiumConstants.AddCustomSkinCost,
                    httpContextReader.CurrentUserId)))
            {
                return await premiumAccountManager.AddCustomSkin(request.SkinFile, request.SkinId, request.CharacterId)
                    ? (AddCustomSkinResponse) new AddCustomSkinResponse().LogInformation(
                        $"user #{httpContextReader.CurrentUserId} has added skin file " +
                        $"for character #{request.CharacterId} with price {PremiumConstants.AddCustomSkinCost}")
                    : throw new PremiumOperationException("Adding custom skin failed");
            }
        }
    }
}