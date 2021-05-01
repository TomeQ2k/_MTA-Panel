using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;

namespace MTA.Infrastructure.Persistence.Database.RestorePoints
{
    public class PremiumCreditsDatabaseRestorePoint : DatabaseRestorePoint, IPremiumCreditsDatabaseRestorePoint
    {
        public PremiumCreditsDatabaseRestorePoint(IDatabaseRestorer databaseRestorer, IServiceProvider services) : base(
            databaseRestorer, services)
        {
        }

        public override async Task<bool> Restore()
        {
            var premiumCreditsRestoreParams = RestoreParams as PremiumCreditsDatabaseRestoreParams;

            if (premiumCreditsRestoreParams == null)
                return false;

            using (var scope = services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();

                return await userManager.AddCredits(premiumCreditsRestoreParams.CreditsToRefund,
                    premiumCreditsRestoreParams.UserId) != null
                    ? await base.Restore()
                    : false;
            }
        }
    }
}