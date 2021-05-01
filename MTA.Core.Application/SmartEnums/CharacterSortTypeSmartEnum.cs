using Ardalis.SmartEnum;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class CharacterSortTypeSmartEnum : SmartEnum<CharacterSortTypeSmartEnum>
    {
        protected CharacterSortTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static CharacterSortTypeSmartEnum IdAscending = new IdAscendingType();
        public static CharacterSortTypeSmartEnum IdDescending = new IdDescendingType();
        public static CharacterSortTypeSmartEnum NameAscending = new NameAscendingType();
        public static CharacterSortTypeSmartEnum NameDescending = new NameDescendingType();
        public static CharacterSortTypeSmartEnum MoneyAscending = new MoneyAscendingType();
        public static CharacterSortTypeSmartEnum MoneyDescending = new MoneyDescendingType();
        public static CharacterSortTypeSmartEnum HoursPlayedAscending = new HoursPlayedAscendingType();
        public static CharacterSortTypeSmartEnum HoursPlayedDescending = new HoursPlayedDescendingType();
        public static CharacterSortTypeSmartEnum LastLoginAscending = new LastLoginAscendingType();
        public static CharacterSortTypeSmartEnum LastLoginDescending = new LastLoginDescendingType();
        public static CharacterSortTypeSmartEnum CreationDateAscending = new CreationDateAscendingType();
        public static CharacterSortTypeSmartEnum CreationDateDescending = new CreationDateDescendingType();
        public static CharacterSortTypeSmartEnum ActiveAscending = new ActiveAscendingType();
        public static CharacterSortTypeSmartEnum ActiveDescending = new ActiveDescendingType();

        public abstract SqlQuery OrderBy();

        private sealed class IdAscendingType : CharacterSortTypeSmartEnum
        {
            public IdAscendingType() : base(nameof(IdAscending),
                (int) CharacterSortType.IdAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.id")
                    .Build();
        }

        private sealed class IdDescendingType : CharacterSortTypeSmartEnum
        {
            public IdDescendingType() : base(nameof(IdDescending),
                (int) CharacterSortType.IdDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.id", OrderByType.Descending)
                    .Build();
        }

        private sealed class NameAscendingType : CharacterSortTypeSmartEnum
        {
            public NameAscendingType() : base(nameof(NameAscending),
                (int) CharacterSortType.IdAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.charactername")
                    .Build();
        }

        private sealed class NameDescendingType : CharacterSortTypeSmartEnum
        {
            public NameDescendingType() : base(nameof(NameDescending),
                (int) CharacterSortType.NameDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.charactername", OrderByType.Descending)
                    .Build();
        }

        private sealed class MoneyAscendingType : CharacterSortTypeSmartEnum
        {
            public MoneyAscendingType() : base(nameof(MoneyAscending),
                (int) CharacterSortType.MoneyAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("TotalMoney")
                    .Build();
        }

        private sealed class MoneyDescendingType : CharacterSortTypeSmartEnum
        {
            public MoneyDescendingType() : base(nameof(MoneyDescending),
                (int) CharacterSortType.MoneyDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("TotalMoney", OrderByType.Descending)
                    .Build();
        }

        private sealed class HoursPlayedAscendingType : CharacterSortTypeSmartEnum
        {
            public HoursPlayedAscendingType() : base(nameof(HoursPlayedAscending),
                (int) CharacterSortType.HoursPlayedAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.hoursplayed")
                    .Build();
        }

        private sealed class HoursPlayedDescendingType : CharacterSortTypeSmartEnum
        {
            public HoursPlayedDescendingType() : base(nameof(HoursPlayedDescending),
                (int) CharacterSortType.HoursPlayedAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.hoursplayed", OrderByType.Descending)
                    .Build();
        }

        private sealed class LastLoginAscendingType : CharacterSortTypeSmartEnum
        {
            public LastLoginAscendingType() : base(nameof(LastLoginAscending),
                (int) CharacterSortType.HoursPlayedAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.lastlogin")
                    .Build();
        }

        private sealed class LastLoginDescendingType : CharacterSortTypeSmartEnum
        {
            public LastLoginDescendingType() : base(nameof(LastLoginDescending),
                (int) CharacterSortType.HoursPlayedAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.lastlogin", OrderByType.Descending)
                    .Build();
        }

        private sealed class CreationDateAscendingType : CharacterSortTypeSmartEnum
        {
            public CreationDateAscendingType() : base(nameof(CreationDateAscending),
                (int) CharacterSortType.CreationDateAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.creationdate")
                    .Build();
        }

        private sealed class CreationDateDescendingType : CharacterSortTypeSmartEnum
        {
            public CreationDateDescendingType() : base(nameof(CreationDateDescending),
                (int) CharacterSortType.CreationDateDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.creationdate", OrderByType.Descending)
                    .Build();
        }

        private sealed class ActiveAscendingType : CharacterSortTypeSmartEnum
        {
            public ActiveAscendingType() : base(nameof(ActiveAscending),
                (int) CharacterSortType.ActiveAscending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.active")
                    .Build();
        }

        private sealed class ActiveDescendingType : CharacterSortTypeSmartEnum
        {
            public ActiveDescendingType() : base(nameof(ActiveDescending),
                (int) CharacterSortType.ActiveDescending)
            {
            }

            public override SqlQuery OrderBy()
                => new SqlBuilder()
                    .OrderBy("p.active", OrderByType.Descending)
                    .Build();
        }
    }
}