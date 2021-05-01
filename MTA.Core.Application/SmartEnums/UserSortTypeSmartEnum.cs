using Ardalis.SmartEnum;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class UserSortTypeSmartEnum : SmartEnum<UserSortTypeSmartEnum>
    {
        protected UserSortTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static UserSortTypeSmartEnum IdAscending = new IdAscendingType();
        public static UserSortTypeSmartEnum IdDescending = new IdDescendingType();
        public static UserSortTypeSmartEnum RegisterDateAscending = new RegisterDateAscendingType();
        public static UserSortTypeSmartEnum RegisterDateDescending = new RegisterDateDescendingType();
        public static UserSortTypeSmartEnum LastLoginAscending = new LastLoginAscendingType();
        public static UserSortTypeSmartEnum LastLoginDescending = new LastLoginDescendingType();

        public abstract SqlQuery OrderBy();

        private sealed class IdAscendingType : UserSortTypeSmartEnum
        {
            public IdAscendingType() : base(nameof(IdAscending),
                (int) UserSortType.IdAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("k.id")
                    .Build();
        }

        private sealed class IdDescendingType : UserSortTypeSmartEnum
        {
            public IdDescendingType() : base(nameof(IdDescending),
                (int) UserSortType.IdDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("k.id", OrderByType.Descending)
                    .Build();
        }

        private sealed class RegisterDateAscendingType : UserSortTypeSmartEnum
        {
            public RegisterDateAscendingType() : base(nameof(RegisterDateAscending),
                (int) UserSortType.RegisterDateAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("k.registerdate")
                    .Build();
        }

        private sealed class RegisterDateDescendingType : UserSortTypeSmartEnum
        {
            public RegisterDateDescendingType() : base(nameof(RegisterDateDescending),
                (int) UserSortType.RegisterDateDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("k.registerdate", OrderByType.Descending)
                    .Build();
        }

        private sealed class LastLoginAscendingType : UserSortTypeSmartEnum
        {
            public LastLoginAscendingType() : base(nameof(LastLoginAscending),
                (int) UserSortType.LastLoginAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("k.lastlogin")
                    .Build();
        }

        private sealed class LastLoginDescendingType : UserSortTypeSmartEnum
        {
            public LastLoginDescendingType() : base(nameof(LastLoginDescending),
                (int) UserSortType.LastLoginDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("k.lastlogin", OrderByType.Descending)
                    .Build();
        }
    }
}