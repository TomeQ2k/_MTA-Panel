using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

#pragma warning disable 693

namespace MTA.Core.Domain.Data
{
    public interface IRepository<T> : IDatabaseExecutor where T : EntityModel
    {
        string Table { get; }

        Task<IEnumerable<T>> Query(SqlQuery query);
        Task<T> QueryFirst(SqlQuery query);

        Task<IEnumerable<T>> QueryJoin<T, TRelation>(SqlQuery query, Func<T, TRelation, T> map,
            string splitOn = null);

        Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2>(SqlQuery query,
            Func<T, TRelation1, TRelation2, T> map, string splitOn = null);

        Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3>(
            SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, T> map,
            string splitOn = null
        );

        Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3, TRelation4>(
            SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, TRelation4, T> map,
            string splitOn = null
        );

        Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5>(
            SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5, T> map,
            string splitOn = null
        );

        Task<IEnumerable<T>> QueryJoin<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5, TRelation6>(
            SqlQuery query,
            Func<T, TRelation1, TRelation2, TRelation3, TRelation4, TRelation5, TRelation6, T> map,
            string splitOn = null
        );

        Task<T> FindById(object id);
        Task<T> FindByColumn(ColumnValue idColumnValue);
        Task<T> Find(string clause);

        Task<IEnumerable<T>> GetAll(int limit = 200);
        Task<IEnumerable<T>> GetWhere(string clause);

        Task<bool> Insert<T>(T entity, bool autoPrimaryKey = true);
        Task<bool> InsertRange<T>(IEnumerable<T> entities, bool autoPrimaryKey = true);

        Task<bool> Update<T>(T entity);
        Task<bool> UpdateRange<T>(IEnumerable<T> entities);

        Task<bool> Delete<T>(T entity);
        Task<bool> DeleteRange<T>(IEnumerable<T> entities);
        Task<bool> DeleteByColumn(ColumnValue columnValue);
        Task<bool> DeleteRangeByColumn(params ColumnValue[] columnValues);
    }
}