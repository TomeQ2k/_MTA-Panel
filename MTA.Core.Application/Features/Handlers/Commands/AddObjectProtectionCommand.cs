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
    public class AddObjectProtectionCommand : IRequestHandler<AddObjectProtectionRequest, AddObjectProtectionResponse>
    {
        private readonly IPremiumAccountManager premiumAccountManager;
        private readonly IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint;
        private readonly IHttpContextReader httpContextReader;

        public AddObjectProtectionCommand(IPremiumAccountManager premiumAccountManager,
            IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint,
            IHttpContextReader httpContextReader)
        {
            this.premiumAccountManager = premiumAccountManager;
            this.premiumCreditsDatabaseRestorePoint = premiumCreditsDatabaseRestorePoint;
            this.httpContextReader = httpContextReader;
        }

        public async Task<AddObjectProtectionResponse> Handle(AddObjectProtectionRequest request,
            CancellationToken cancellationToken)
        {
            using (var _ = premiumCreditsDatabaseRestorePoint.CreateRestoreParams(
                new PremiumCreditsDatabaseRestoreParams(PremiumConstants.AddObjectProtectionCost * request.Amount,
                    httpContextReader.CurrentUserId)))
            {
                var objectProtectionResult = await premiumAccountManager.AddObjectProtection(request)
                                             ?? throw new PremiumOperationException(
                                                 "Premium operation of adding object protection failed");

                return objectProtectionResult.IsSucceeded
                    ? (AddObjectProtectionResponse) new AddObjectProtectionResponse
                    {
                        ProtectionType = objectProtectionResult.ProtectionType,
                        ObjectId = objectProtectionResult.ObjectId,
                        ProtectedUntil = objectProtectionResult.ProtectedUntil
                    }.LogInformation(
                        $"Protection has been added to object #{request.ObjectId} " +
                        $"by user #{httpContextReader.CurrentUserId} with price: {request.Amount * PremiumConstants.AddObjectProtectionCost} " +
                        $"for {request.Amount} days")
                    : throw new PremiumOperationException("Premium operation of adding object protection failed");
            }
        }
    }
}