using System;

namespace MTA.Core.Domain.Data.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
    {
        public ColumnAttribute(string name, bool primaryKey = false, bool customProperty = false) : base(name)
            => (PrimaryKey, CustomProperty) = (primaryKey, customProperty);

        public bool PrimaryKey { get; }
        public bool CustomProperty { get; }
    }
}