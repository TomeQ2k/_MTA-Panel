using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface ISqlBuilder : IBuilder<SqlQuery>
    {
        ISqlBuilder And { get; }
        ISqlBuilder Or { get; }
        ISqlBuilder Not { get; }
        ISqlBuilder Equals { get; }
        ISqlBuilder NotEquals { get; }
        ISqlBuilder Greater { get; }
        ISqlBuilder Lesser { get; }
        ISqlBuilder GreaterEquals { get; }
        ISqlBuilder LesserEquals { get; }
        ISqlBuilder Open { get; }
        ISqlBuilder Close { get; }

        ISqlBuilder Add { get; }
        ISqlBuilder Minus { get; }
        ISqlBuilder Multiply { get; }
        ISqlBuilder Divide { get; }

        ISqlBuilder Case { get; }
        ISqlBuilder End { get; }

        ISqlBuilder Between { get; }

        ISqlBuilder IsNull { get; }
        ISqlBuilder IsNotNull { get; }

        ISqlBuilder True { get; }
        ISqlBuilder False { get; }

        ISqlBuilder Append(object value);

        ISqlBuilder Select(bool all = true);
        ISqlBuilder Select(params string[] columns);
        ISqlBuilder Select<T1>(params string[] columnsToIgnore);

        ISqlBuilder SelectDistinct();
        ISqlBuilder SelectCount();

        ISqlBuilder From(string table);

        ISqlBuilder Where(string clause = null);
        ISqlBuilder WhereIn<TValue>(string column, TValue[] values);
        ISqlBuilder WhereAny(string column, SqlQuery query);
        ISqlBuilder WhereAll(string column, SqlQuery query);
        ISqlBuilder WhereBetween(string column, object firstValue, object secondValue);
        ISqlBuilder WhereLike(string column, string pattern);
        ISqlBuilder WhereNull(string column);
        ISqlBuilder WhereNotNull(string column);

        ISqlBuilder Exists(SqlQuery query);
        ISqlBuilder WhereExists(SqlQuery query);

        ISqlBuilder OrderBy(string column = null, OrderByType orderByType = OrderByType.Ascending);
        ISqlBuilder OrderBy(SqlQuery query);

        ISqlBuilder GroupBy(params string[] columns);
        ISqlBuilder Having(string clause = null);

        ISqlBuilder Join(string index, JoinIndex joinIndex, string joinTableAlias = "joinTable");
        ISqlBuilder LeftJoin(string index, JoinIndex joinIndex, string joinTableAlias = "joinTable");
        ISqlBuilder RightJoin(string index, JoinIndex joinIndex, string joinTableAlias = "joinTable");

        ISqlBuilder Limit(int count);
        ISqlBuilder Pagination(int pageNumber, int pageSize);

        ISqlBuilder Count(string clause);
        ISqlBuilder Sum(string clause);

        ISqlBuilder As(string alias);

        ISqlBuilder If(object condition, string clause, string column);

        ISqlBuilder When(object clause);
        ISqlBuilder Then(object clause);

        ISqlBuilder Like(string pattern = null);

        ISqlBuilder In<TValue>(TValue[] values);

        ISqlBuilder Concat(params string[] values);

        ISqlBuilder InsertInto(string table, ColumnValueDictionary columnValues);
        ISqlBuilder Update(string table, ColumnValueDictionary columnValues, ColumnValue idColumnValue);
        ISqlBuilder Delete(string table, ColumnValue idColumnValue = null, bool where = true);
        ISqlBuilder DeleteRange(string table, params ColumnValue[] idColumnValues);
    }
}