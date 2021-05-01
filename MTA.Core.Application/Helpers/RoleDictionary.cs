using System.Collections.Generic;
using System.Linq;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Application.Helpers
{
    public static class RoleDictionary
    {
        public static ColumnValue DefaultRoleColumnValue(string column) => new ColumnValue(column, 0);

        public static Dictionary<RoleType, ColumnValue> RolesDictionary => new Dictionary<RoleType, ColumnValue>()
        {
            {RoleType.Owner, new ColumnValue("admin", 4)},
            {RoleType.ViceOwner, new ColumnValue("admin", 3)},
            {RoleType.Admin, new ColumnValue("admin", 2)},
            {RoleType.TrialAdmin, new ColumnValue("admin", 1)},
            {RoleType.SupporterLeader, new ColumnValue("supporter", 2)},
            {RoleType.Supporter, new ColumnValue("supporter", 1)},
            {RoleType.VctLeader, new ColumnValue("vct", 2)},
            {RoleType.Vct, new ColumnValue("vct", 1)},
            {RoleType.MapperLeader, new ColumnValue("mapper", 2)},
            {RoleType.Mapper, new ColumnValue("mapper", 1)},
            {RoleType.Tester, new ColumnValue("scripter", 1)},
            {RoleType.TrialScripter, new ColumnValue("scripter", 2)},
            {RoleType.Scripter, new ColumnValue("scripter", 3)}
        };

        public static RoleType FindRoleTypeByUserRole(ColumnValue columnValue)
            => RolesDictionary
                .FirstOrDefault(x =>
                    (x.Value.Column, (int) x.Value.Value) == (columnValue.Column, (int) columnValue.Value)).Key;
    }
}