using Ardalis.SmartEnum;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Helpers;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class RoleTypeSmartEnum : SmartEnum<RoleTypeSmartEnum>
    {
        protected RoleTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static readonly RoleTypeSmartEnum Owner = new OwnerType();
        public static readonly RoleTypeSmartEnum ViceOwner = new ViceOwnerType();
        public static readonly RoleTypeSmartEnum Admin = new AdminType();
        public static readonly RoleTypeSmartEnum TrialAdmin = new TrialAdminType();
        public static readonly RoleTypeSmartEnum SupporterLeader = new SupporterLeaderType();
        public static readonly RoleTypeSmartEnum Supporter = new SupporterType();
        public static readonly RoleTypeSmartEnum VctLeader = new VctLeaderType();
        public static readonly RoleTypeSmartEnum Vct = new VctType();
        public static readonly RoleTypeSmartEnum MapperLeader = new MapperLeaderType();
        public static readonly RoleTypeSmartEnum Mapper = new MapperType();
        public static readonly RoleTypeSmartEnum Scripter = new ScripterType();

        private static readonly string userTable = RepositoryDictionary.FindTable(typeof(IUserRepository));

        private static ColumnValue IdColumnValue(int id) => new ColumnValue("id", id);

        private static SqlQuery BuildAdmitQuery(int id, ColumnValue columnValue)
            => new SqlBuilder()
                .Update(userTable, ColumnValueDictionary.Create(columnValue),
                    IdColumnValue(id))
                .Build();

        private static SqlQuery BuildRevokeQuery(int id, string column)
            => new SqlBuilder()
                .Update(userTable, ColumnValueDictionary.Create(RoleDictionary.DefaultRoleColumnValue(column)),
                    IdColumnValue(id))
                .Build();

        private static SqlQuery BuildIsPermittedQuery(ColumnValue columnValue)
            => new SqlBuilder()
                .Append(columnValue.Column)
                .Equals
                .Append(columnValue.Value)
                .Or
                .Build();

        public abstract SqlQuery Admit(int id);
        public abstract SqlQuery Revoke(int id);
        public abstract SqlQuery IsPermitted();

        private sealed class OwnerType : RoleTypeSmartEnum
        {
            public OwnerType() : base(nameof(Owner), (int) RoleType.Owner)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.Owner]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.Owner].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.Owner]);
        }

        private sealed class ViceOwnerType : RoleTypeSmartEnum
        {
            public ViceOwnerType() : base(nameof(ViceOwner), (int) RoleType.ViceOwner)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.ViceOwner]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.ViceOwner].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.ViceOwner]);
        }

        private sealed class AdminType : RoleTypeSmartEnum
        {
            public AdminType() : base(nameof(Admin), (int) RoleType.Admin)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.Admin]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.Admin].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.Admin]);
        }

        private sealed class TrialAdminType : RoleTypeSmartEnum
        {
            public TrialAdminType() : base(nameof(TrialAdmin), (int) RoleType.TrialAdmin)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.TrialAdmin]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.TrialAdmin].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.TrialAdmin]);
        }

        private sealed class SupporterLeaderType : RoleTypeSmartEnum
        {
            public SupporterLeaderType() : base(nameof(SupporterLeader), (int) RoleType.SupporterLeader)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.SupporterLeader]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.SupporterLeader].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.SupporterLeader]);
        }

        private sealed class SupporterType : RoleTypeSmartEnum
        {
            public SupporterType() : base(nameof(Supporter), (int) RoleType.Supporter)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.Supporter]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.Supporter].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.Supporter]);
        }

        private sealed class VctLeaderType : RoleTypeSmartEnum
        {
            public VctLeaderType() : base(nameof(VctLeader), (int) RoleType.VctLeader)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.VctLeader]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.VctLeader].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.VctLeader]);
        }

        private sealed class VctType : RoleTypeSmartEnum
        {
            public VctType() : base(nameof(Vct), (int) RoleType.Vct)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.Vct]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.Vct].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.Vct]);
        }

        private sealed class MapperLeaderType : RoleTypeSmartEnum
        {
            public MapperLeaderType() : base(nameof(MapperLeader), (int) RoleType.MapperLeader)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.MapperLeader]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.MapperLeader].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.MapperLeader]);
        }

        private sealed class MapperType : RoleTypeSmartEnum
        {
            public MapperType() : base(nameof(Mapper), (int) RoleType.Mapper)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.Mapper]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.Mapper].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.Mapper]);
        }

        private sealed class ScripterType : RoleTypeSmartEnum
        {
            public ScripterType() : base(nameof(Scripter), (int) RoleType.Scripter)
            {
            }

            public override SqlQuery Admit(int id)
                => BuildAdmitQuery(id, RoleDictionary.RolesDictionary[RoleType.Scripter]);

            public override SqlQuery Revoke(int id)
                => BuildRevokeQuery(id, RoleDictionary.RolesDictionary[RoleType.Scripter].Column);

            public override SqlQuery IsPermitted()
                => BuildIsPermittedQuery(RoleDictionary.RolesDictionary[RoleType.Scripter]);
        }
    }
}