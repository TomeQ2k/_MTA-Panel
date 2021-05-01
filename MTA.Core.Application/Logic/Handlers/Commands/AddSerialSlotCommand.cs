using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class AddSerialSlotCommand : IRequestHandler<AddSerialSlotRequest, AddSerialSlotResponse>
    {
        private readonly IPremiumAccountManager premiumAccountManager;
        private readonly IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint;
        private readonly IHttpContextReader httpContextReader;

        public AddSerialSlotCommand(IPremiumAccountManager premiumAccountManager,
            IPremiumCreditsDatabaseRestorePoint premiumCreditsDatabaseRestorePoint,
            IHttpContextReader httpContextReader)
        {
            this.premiumAccountManager = premiumAccountManager;
            this.premiumCreditsDatabaseRestorePoint = premiumCreditsDatabaseRestorePoint;
            this.httpContextReader = httpContextReader;
        }

        public async Task<AddSerialSlotResponse> Handle(AddSerialSlotRequest request,
            CancellationToken cancellationToken)
        {
            using (var _ = premiumCreditsDatabaseRestorePoint.CreateRestoreParams(
                    new PremiumCreditsDatabaseRestoreParams(PremiumConstants.AddSerialSlotCost * request.Amount,
                        httpContextReader.CurrentUserId))
                .EnqueueToConnectionDatabaseRestorePoints(httpContextReader.ConnectionId))
            {
                var addSerialSlotResult = await premiumAccountManager.AddSerialSlot(request)
                                          ?? throw new PremiumOperationException(
                                              "Premium operation of adding serial slot failed");

                return addSerialSlotResult.IsSucceeded
                    ? (AddSerialSlotResponse) new AddSerialSlotResponse
                            {CurrentSerialsLimit = addSerialSlotResult.CurrentSerialsLimit}
                        .LogInformation(
                            $"{(request.Amount == 1 ? "Serial slot has" : $"{request.Amount} Serial slots have")} " +
                            $"been added to user #{httpContextReader.CurrentUserId} with price: {request.Amount * PremiumConstants.AddSerialSlotCost}")
                    : throw new PremiumOperationException("Premium operation of adding serial slot failed");
            }
        }
    }
}