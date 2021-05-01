using System.Linq;
using System.Reflection;
using Dapper;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.API.AppConfigs
{
    public static class DapperAppConfig
    {
        private const string EntitiesAssembly = "MTA.Core.Domain";

        public static void ConfigureSqlMapper()
        {
            var entitiesTypes = from type in Assembly.Load(EntitiesAssembly).GetTypes()
                where type.IsClass && type.BaseType == typeof(EntityModel)
                select type;

            entitiesTypes.ToList().ForEach(type =>
            {
                SqlMapper.SetTypeMap(
                    type,
                    new CustomPropertyTypeMap(
                        type,
                        (t, columnName) =>
                            t.GetProperties().FirstOrDefault(prop =>
                                prop.GetCustomAttributes(false)
                                    .OfType<ColumnAttribute>()
                                    .Any(attr => attr.Name == columnName))));
            });
        }
    }
}