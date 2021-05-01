using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MTA.Core.Domain.Data.Helpers
{
    public class ColumnValueDictionary : Dictionary<string, object>
    {
        private static ColumnValueDictionary columnValuesDictionary { get; } = new();

        public static ColumnValueDictionary Create(params ColumnValue[] columnValues)
        {
            columnValuesDictionary.Clear();

            foreach (var columnValue in columnValues)
                columnValuesDictionary.Add(columnValue.Column, columnValue.Value);

            return columnValuesDictionary;
        }

        public static ColumnValueDictionary Create<T>(T entity, IEnumerable<PropertyInfo> entityProperties,
            bool autoPrimaryKey = true)
        {
            columnValuesDictionary.Clear();

            entityProperties.ToList().ForEach(ep =>
            {
                var columnValue = new ColumnValue(ep.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>()
                    .FirstOrDefault(a => autoPrimaryKey ? !a.PrimaryKey : true)?.Name, ep.GetValue(entity));

                if (columnValue.Column != null)
                    columnValuesDictionary.Add($"`{columnValue.Column}`", columnValue.Value);
            });

            return columnValuesDictionary;
        }
    }
}