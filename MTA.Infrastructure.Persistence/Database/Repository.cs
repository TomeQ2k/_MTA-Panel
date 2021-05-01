using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

#pragma warning disable 693

namespace MTA.Infrastructure.Persistence.Database
{
    public class Repository<T> : IRepository<T> where T : EntityModel
    {
        protected readonly ISqlConnectionFactory connectionFactory;

        public string Table { get; }

        public Repository(ISqlConnectionFactory connectionFactory, string table)
        {
            this.connectionFactory = connectionFactory;

            Table = table;
        }

        public async Task<IEnumerable<T>> Query(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync<T>(query.Query, commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<T> QueryFirst(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return (await connection.QueryAsync<T>(query.Query, commandTimeout: SqlConstants.CommandTimeout))
                    .FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> QueryJoin<T, TRelation>(SqlQuery query, Func<T, TRelation, T> map,
            string splitOn = null)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync(query.Query, map, splitOn: splitOn ?? "id",
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2>(SqlQuery query,
            Func<T, TRelation1, TRelation2, T> map, string splitOn = null)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync(query.Query, map, splitOn: splitOn ?? "id",
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3>(SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, T> map, string splitOn = null)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync(query.Query, map, splitOn: splitOn ?? "id",
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3, TRelation4>(SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, TRelation4, T> map, string splitOn = null)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync(query.Query, map, splitOn: splitOn ?? "id",
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5>(
            SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5, T> map, string splitOn = null)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync(query.Query, map, splitOn: splitOn ?? "id",
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5,
            TRelation6>(SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5, TRelation6, T> map,
            string splitOn = null)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync(query.Query, map, splitOn: splitOn ?? "id",
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<T> FindById(object id)
        {
            using (var connection = connectionFactory.Connection)
            {
                string idColumn = Activator.CreateInstance<T>().GetPropertiesWithAttribute(typeof(ColumnAttribute))
                    .FindPrimaryKeyProperty().GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()
                    .Name;

                var findByIdQuery = new SqlBuilder()
                    .Select()
                    .From(Table)
                    .Where(idColumn).Equals.Append($"'{id}'")
                    .Limit(1)
                    .Build();

                return (await connection.QueryAsync<T>(findByIdQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout)).FirstOrDefault();
            }
        }

        public async Task<T> FindByColumn(ColumnValue idColumnValue)
        {
            using (var connection = connectionFactory.Connection)
            {
                var findByColumnQuery = new SqlBuilder()
                    .Select()
                    .From(Table)
                    .Where(idColumnValue.Column).Equals.Append($"'{idColumnValue.Value}'")
                    .Limit(1)
                    .Build();

                return (await connection.QueryAsync<T>(findByColumnQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout)).FirstOrDefault();
            }
        }

        public async Task<T> Find(string clause)
        {
            using (var connection = connectionFactory.Connection)
            {
                var findQuery = new SqlBuilder()
                    .Select()
                    .From(Table)
                    .Where(clause)
                    .Limit(1)
                    .Build();

                return (await connection.QueryAsync<T>(findQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout)).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetAll(int limit = 200)
        {
            using (var connection = connectionFactory.Connection)
            {
                var getAllQuery = new SqlBuilder()
                    .Select()
                    .From(Table)
                    .Limit(limit)
                    .Build();

                return await connection.QueryAsync<T>(getAllQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<IEnumerable<T>> GetWhere(string clause)
        {
            using (var connection = connectionFactory.Connection)
            {
                var getWhereQuery = new SqlBuilder()
                    .Select()
                    .From(Table)
                    .Where(clause)
                    .Build();

                return await connection.QueryAsync<T>(getWhereQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout);
            }
        }

        public async Task<bool> Insert<T>(T entity, bool autoPrimaryKey = true)
        {
            var entityProperties = entity.GetPropertiesWithAttribute(typeof(ColumnAttribute))
                .Where(ep => !ep.GetCustomAttributes().OfType<ColumnAttribute>().FirstOrDefault().CustomProperty);

            var columnValues = ColumnValueDictionary.Create(entity, entityProperties, autoPrimaryKey);

            using (var connection = connectionFactory.Connection)
            {
                var insertQuery = new SqlBuilder()
                    .InsertInto(Table, columnValues)
                    .Build();

                return await connection.ExecuteAsync(insertQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<bool> InsertRange<T>(IEnumerable<T> entities, bool autoPrimaryKey = true)
        {
            bool isInserted = true;

            using (var transaction = BeginTransaction().Transaction)
            {
                foreach (var entity in entities)
                    isInserted = isInserted && await Insert(entity, autoPrimaryKey);

                transaction.Complete();
            }

            return isInserted;
        }

        public async Task<bool> Update<T>(T entity)
        {
            var entityProperties = entity.GetPropertiesWithAttribute(typeof(ColumnAttribute))
                .Where(ep => !ep.GetCustomAttributes().OfType<ColumnAttribute>().FirstOrDefault().CustomProperty);

            var entityPrimaryKey = FindEntityPrimaryKey(entity, entityProperties);

            var columnValues = ColumnValueDictionary.Create(entity, entityProperties);

            using (var connection = connectionFactory.Connection)
            {
                var updateQuery = new SqlBuilder()
                    .Update(Table, columnValues, entityPrimaryKey)
                    .Build();

                return await connection.ExecuteAsync(updateQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<bool> UpdateRange<T>(IEnumerable<T> entities)
        {
            bool isUpdated = true;

            using (var transaction = BeginTransaction().Transaction)
            {
                foreach (var entity in entities)
                    isUpdated = isUpdated && await Update(entity);

                transaction.Complete();
            }

            return isUpdated;
        }

        public async Task<bool> Delete<T>(T entity)
        {
            var entityProperties = entity.GetPropertiesWithAttribute(typeof(ColumnAttribute));
            var primaryKeyProperties = entityProperties.FindPrimaryKeyProperties().ToList();

            using (var connection = connectionFactory.Connection)
            {
                var deleteQuery = new SqlBuilder()
                    .Delete(Table)
                    .Where();

                for (int i = 0; i < primaryKeyProperties.Count; i++)
                {
                    if (i != 0)
                        deleteQuery = deleteQuery.And;

                    deleteQuery = deleteQuery
                        .Append(
                            $"`{primaryKeyProperties[i].GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault().Name}`")
                        .Equals
                        .Append($"'{primaryKeyProperties[i].GetValue(entity)}'");
                }

                return await connection.ExecuteAsync(deleteQuery.Build().Query,
                    commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<bool> DeleteRange<T1>(IEnumerable<T1> entities)
        {
            bool isDeleted = true;

            using (var transaction = BeginTransaction().Transaction)
            {
                foreach (var entity in entities)
                    isDeleted = isDeleted && await Delete(entity);

                transaction.Complete();
            }

            return isDeleted;
        }

        public async Task<bool> DeleteByColumn(ColumnValue columnValue)
        {
            using (var connection = connectionFactory.Connection)
            {
                var deleteQuery = new SqlBuilder()
                    .Delete(Table, columnValue)
                    .Build();

                return await connection.ExecuteAsync(deleteQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<bool> DeleteRangeByColumn(params ColumnValue[] columnValues)
        {
            using (var connection = connectionFactory.Connection)
            {
                var deleteRangeQuery = new SqlBuilder()
                    .DeleteRange(Table, columnValues)
                    .Build();

                return await connection.ExecuteAsync(deleteRangeQuery.Query,
                    commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<bool> Execute(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.ExecuteAsync(query.Query, commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<IEnumerable<TValue>> SelectQuery<TValue>(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync<TValue>(query.Query, commandTimeout: SqlConstants
                    .CommandTimeout);
            }
        }

        public async Task<TValue> SelectQueryFirst<TValue>(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return (await connection.QueryAsync<TValue>(query.Query, commandTimeout: SqlConstants.CommandTimeout))
                    .FirstOrDefault();
            }
        }

        public IDatabaseTransaction BeginTransaction() => new DatabaseTransaction();

        #region private

        private static ColumnValue FindEntityPrimaryKey<T>(T entity, IEnumerable<PropertyInfo> entityProperties)
        {
            var entityPrimaryKeyProperty = entityProperties.FindPrimaryKeyProperty();
            var entityPrimaryKey =
                new ColumnValue(
                    $"`{entityPrimaryKeyProperty?.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name}`",
                    entityPrimaryKeyProperty?.GetValue(entity));

            return entityPrimaryKey;
        }

        #endregion
    }
}