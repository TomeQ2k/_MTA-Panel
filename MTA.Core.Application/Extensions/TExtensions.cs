using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MTA.Core.Application.Extensions
{
    public static class TExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this T entity, Type attributeType)
            => entity.GetType().GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == attributeType));
    }
}