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
    public class AddCustomInteriorCommand : IRequestHandler<AddCustomInteriorRequest, AddCustomInteriorResponse>
    {
        private readonly IPremiumAccountManager premiumAccountManager;
        private readonly IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint;
        private readonly IHttpContextReader httpContextReader;

        public AddCustomInteriorCommand(IPremiumAccountManager premiumAccountManager,
            IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint,
            IHttpContextReader httpContextReader)
        {
            this.premiumAccountManager = premiumAccountManager;
            this.premiumCreditsDatabaseRestorePoint = premiumCreditsDatabaseRestorePoint;
            this.httpContextReader = httpContextReader;
        }

        public async Task<AddCustomInteriorResponse> Handle(AddCustomInteriorRequest request,
            CancellationToken cancellationToken)
        {
            using (var _ = premiumCreditsDatabaseRestorePoint.CreateRestoreParams(
                new PremiumCreditsDatabaseRestoreParams(PremiumConstants.AddCustomInteriorCost,
                    httpContextReader.CurrentUserId)))
            {
                return await premiumAccountManager.AddCustomInterior(request.InteriorFile, request.EstateId)
                    ? (AddCustomInteriorResponse) new AddCustomInteriorResponse().LogInformation(
                        $"user #{httpContextReader.CurrentUserId} has added interior file " +
                        $"for estate #{request.EstateId} with price {PremiumConstants.AddCustomInteriorCost}")
                    : throw new PremiumOperationException("Adding custom interior failed");
            }
        }
    }
}