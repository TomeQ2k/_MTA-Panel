using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Data;

namespace MTA.Core.Application.Filters
{
    public class PremiumFilter : IAsyncActionFilter
    {
        public int Cost { get; set; }

        private const string AmountKey = "amount";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext result;

            Cost *= context.HttpContext.Request.Form.ContainsKey(AmountKey)
                ? int.Parse(context.HttpContext.Request.Form[AmountKey])
                : 1;

            var (database, httpContextReader, configuration) = (
                context.HttpContext.RequestServices.GetService<IDatabase>(),
                context.HttpContext.RequestServices.GetService<IHttpContextReader>(),
                context.HttpContext.RequestServices.GetService<IConfiguration>());

            var currentUser = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                              throw new AuthException("User not authorized");

            bool isOwner = RoleDictionary.FindRoleTypeByUserRole(new("admin", currentUser.AdminRole)) == RoleType.Owner
                           || configuration.IsDev(httpContextReader.CurrentUserId);

            var hasToPayPermission = PermissionModel<PremiumPermission>.Create(PremiumPermission.HasToPay)
                .AppendPermission(() => !isOwner && currentUser.Credits >= Cost);
            var hasFreePremiumPermission = PermissionModel<PremiumPermission>.Create(PremiumPermission.HasFreePremium)
                .AppendPermission(() => isOwner);

            if (hasFreePremiumPermission.IsPermitted)
            {
                result = await next();
                return;
            }
            else
            {
                if (!hasToPayPermission.IsPermitted)
                    throw new PremiumOperationException("You have not sufficient credits on your account");
                else
                {
                    currentUser.AddCredits(-Cost);

                    if (!await database.UserRepository.Update(currentUser))
                        throw new DatabaseException();
                }
            }

            result = await next();
        }
    }
}