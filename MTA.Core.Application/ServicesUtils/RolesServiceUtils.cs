using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.ServicesUtils
{
    public static class RolesServiceUtils
    {
        private const string AdminRole = "admin";
        private const string SupporterRole = "supporter";
        private const string VctRole = "vct";
        private const string MapperRole = "mapper";
        private const string ScripterRole = "scripter";

        public static SqlQuery BuildIsPermittedQuery(int userId, params RoleType[] roleTypes)
        {
            string rolesConditions = string.Empty;

            roleTypes.ToList()
                .ForEach(r => rolesConditions += RoleTypeSmartEnum.FromValue((int) r).IsPermitted().Query);

            return new SqlBuilder()
                .Exists(new SqlBuilder()
                    .Select()
                    .From(RepositoryDictionary.FindTable(typeof(IUserRepository)))
                    .Where("id")
                    .Equals
                    .Append(userId)
                    .And
                    .Open
                    .Append(rolesConditions)
                    .Build())
                    .Close
                .As("IsPermitted")
                .Build();
        }

        public static List<Claim> GetRoleClaims(User user)
        {
            var roleClaims = new List<Claim>();

            if (user == null) return roleClaims;

            if (user.AdminRole > 0)
                roleClaims.Add(new Claim(ClaimTypes.Role, user.AdminRole switch
                {
                    4 => GetRoleClaimValue(4, AdminRole),
                    3 => GetRoleClaimValue(3, AdminRole),
                    2 => GetRoleClaimValue(2, AdminRole),
                    1 => GetRoleClaimValue(1, AdminRole),
                    _ => throw new ArgumentOutOfRangeException()
                }));

            if (user.SupporterRole > 0)
                roleClaims.Add(new Claim(ClaimTypes.Role, user.SupporterRole switch
                {
                    2 => GetRoleClaimValue(2, SupporterRole),
                    1 => GetRoleClaimValue(1, SupporterRole),
                    _ => throw new ArgumentOutOfRangeException()
                }));

            if (user.VctRole > 0)
                roleClaims.Add(new Claim(ClaimTypes.Role, user.VctRole switch
                {
                    2 => GetRoleClaimValue(2, VctRole),
                    1 => GetRoleClaimValue(1, VctRole),
                    _ => throw new ArgumentOutOfRangeException()
                }));

            if (user.MapperRole > 0)
                roleClaims.Add(new Claim(ClaimTypes.Role, user.MapperRole switch
                {
                    2 => GetRoleClaimValue(2, MapperRole),
                    1 => GetRoleClaimValue(1, MapperRole),
                    _ => throw new ArgumentOutOfRangeException()
                }));

            if (user.ScripterRole > 0)
                roleClaims.Add(new Claim(ClaimTypes.Role, user.ScripterRole switch
                {
                    3 => GetRoleClaimValue(3, ScripterRole),
                    2 => GetRoleClaimValue(2, ScripterRole),
                    1 => GetRoleClaimValue(1, ScripterRole),
                    _ => throw new ArgumentOutOfRangeException()
                }));

            return roleClaims;
        }

        #region private

        private static string GetRoleClaimValue(int value, string roleName)
            => Utils.EnumToString(RoleDictionary.RolesDictionary
                .FirstOrDefault(r => r.Value == new ColumnValue(roleName, value)).Key);

        #endregion
    }
}