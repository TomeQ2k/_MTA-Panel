using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Application.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static PropertyInfo FindPrimaryKeyProperty(this IEnumerable<PropertyInfo> entityProperties)
            => entityProperties.FirstOrDefault(ep => ep.GetCustomAttributes(false)
                .OfType<ColumnAttribute>().FirstOrDefault(a => a.PrimaryKey) != null);

        public static IEnumerable<PropertyInfo> FindPrimaryKeyProperties(
            this IEnumerable<PropertyInfo> entityProperties)
            => entityProperties.Where(ep => ep.GetCustomAttributes(false)
                .OfType<ColumnAttribute>().FirstOrDefault(a => a.PrimaryKey) != null);
    }
}