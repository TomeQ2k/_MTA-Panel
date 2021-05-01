using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class ApiLogSortTypeSmartEnum : SmartEnum<ApiLogSortTypeSmartEnum>
    {
        protected ApiLogSortTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static readonly ApiLogSortTypeSmartEnum DateDescending = new DateDescendingType();
        public static readonly ApiLogSortTypeSmartEnum DateAscending = new DateAscendingType();

        public abstract IEnumerable<ApiLogModel> Sort(IEnumerable<ApiLogModel> logs);

        private sealed class DateDescendingType : ApiLogSortTypeSmartEnum
        {
            public DateDescendingType() : base(nameof(DateDescending), (int) ApiLogSortType.DateDescending)
            {
            }

            public override IEnumerable<ApiLogModel> Sort(IEnumerable<ApiLogModel> logs)
                => logs.OrderByDescending(l => l.Date);
        }

        private sealed class DateAscendingType : ApiLogSortTypeSmartEnum
        {
            public DateAscendingType() : base(nameof(DateAscending), (int) ApiLogSortType.DateAscending)
            {
            }

            public override IEnumerable<ApiLogModel> Sort(IEnumerable<ApiLogModel> logs)
                => logs.OrderBy(l => l.Date);
        }
    }
}