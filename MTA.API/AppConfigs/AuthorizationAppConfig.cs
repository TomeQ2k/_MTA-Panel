using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Common.Helpers;

namespace MTA.API.AppConfigs
{
    public static class AuthorizationAppConfig
    {
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
            => services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.OwnerPolicy,
                    policy => policy.RequireRole(Utils.EnumToString(Constants.OwnerRole)));
                options.AddPolicy(Constants.AdminsPolicy,
                    policy => policy.RequireRole(Constants.AdminRoles.Select(r => Utils.EnumToString(r))));
                options.AddPolicy(Constants.AllAdminsPolicy,
                    policy => policy.RequireRole(Constants.AllAdminsRoles.Select(r => Utils.EnumToString(r))));
                options.AddPolicy(Constants.AllOwnersPolicy,
                    policy => policy.RequireRole(Constants.AllOwnersRoles.Select(r => Utils.EnumToString(r))));
                options.AddPolicy(Constants.AdminsAndSupportersPolicy,
                    policy => policy.RequireRole(
                        Constants.AdminsAndSupportersRoles.Select(r => Utils.EnumToString(r))));
                options.AddPolicy(Constants.TeamPolicy,
                    policy => policy.RequireRole(Constants.TeamRoles.Select(r => Utils.EnumToString(r))));
            });
    }
}