using System;
using System.Linq;
using System.Text.RegularExpressions;
using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Application.Extensions;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Application.Builders
{
    public class SqlBuilder : ISqlBuilder
    {
        private string query = string.Empty;

        public ISqlBuilder And
        {
            get
            {
                query += " AND ";
                return this;
            }
        }

        public ISqlBuilder Or
        {
            get
            {
                query += " OR ";
                return this;
            }
        }

        public ISqlBuilder Not
        {
            get
            {
                query += " NOT ";
                return this;
            }
        }

        public new ISqlBuilder Equals
        {
            get
            {
                query += " = ";
                return this;
            }
        }

        public ISqlBuilder NotEquals
        {
            get
            {
                query += " != ";
                return this;
            }
        }

        public ISqlBuilder Greater
        {
            get
            {
                query += " > ";
                return this;
            }
        }

        public ISqlBuilder Lesser
        {
            get
            {
                query += " < ";
                return this;
            }
        }

        public ISqlBuilder GreaterEquals
        {
            get
            {
                query += " >= ";
                return this;
            }
        }

        public ISqlBuilder LesserEquals
        {
            get
            {
                query += " <= ";
                return this;
            }
        }

        public ISqlBuilder Open
        {
            get
            {
                query += " ( ";
                return this;
            }
        }

        public ISqlBuilder Close
        {
            get
            {
                query += " ) ";
                return this;
            }
        }

        public ISqlBuilder Add
        {
            get
            {
                query += " + ";
                return this;
            }
        }

        public ISqlBuilder Minus
        {
            get
            {
                query += " - ";
                return this;
            }
        }

        public ISqlBuilder Multiply
        {
            get
            {
                query += " * ";
                return this;
            }
        }

        public ISqlBuilder Divide
        {
            get
            {
                query += " / ";
                return this;
            }
        }

        public ISqlBuilder Case
        {
            get
            {
                query += " CASE ";
                return this;
            }
        }

        public ISqlBuilder End
        {
            get
            {
                query += " END ";
                return this;
            }
        }

        public ISqlBuilder Between
        {
            get
            {
                query += " BETWEEN ";
                return this;
            }
        }

        public ISqlBuilder IsNull
        {
            get
            {
                query += " IS NULL ";
                return this;
            }
        }

        public ISqlBuilder IsNotNull
        {
            get
            {
                query += " IS NOT NULL ";
                return this;
            }
        }

        public ISqlBuilder True
        {
            get
            {
                query += " TRUE ";
                return this;
            }
        }

        public ISqlBuilder False
        {
            get
            {
                query += " FALSE ";
                return this;
            }
        }

        public ISqlBuilder Append(object value)
        {
            query += $" {value ?? "NULL"} ";
            return this;
        }

        public ISqlBuilder Select(bool all = true)
        {
            query += $"SELECT {(all ? "*" : string.Empty)} ";
            return this;
        }

        public ISqlBuilder Select(params string[] columns)
        {
            query += $"SELECT  ";

            columns.ToList().ForEach(c => query += $"{c},");
            query = query.RemoveLastCharacter();

            return this;
        }

        public ISqlBuilder Select<T1>(params string[] columnsToIgnore)
        {
            query += $"SELECT  ";

            AppendColumns<T1>(ref query, columnsToIgnore);

            return this;
        }

        public ISqlBuilder SelectDistinct()
        {
            query += "SELECT DISTINCT * ";
            return this;
        }

        public ISqlBuilder SelectCount()
        {
            query += "SELECT COUNT(*) ";
            return this;
        }

        public ISqlBuilder From(string table)
        {
            query += $" FROM {table} ";
            return this;
        }

        public ISqlBuilder Where(string clause = null)
        {
            query += $"WHERE {clause ?? string.Empty} ";
            return this;
        }

        public ISqlBuilder WhereIn<TValue>(string column, TValue[] values)
        {
            var inValues = string.Empty;

            values.ToList().ForEach(v => inValues += $"'{v}',");
            inValues = inValues.RemoveLastCharacter();

            query += $"WHERE {column} IN ({inValues}) ";

            return this;
        }

        public ISqlBuilder WhereAny(string column, SqlQuery query)
        {
            this.query += $"WHERE {column} = ANY ({query.Query}) ";
            return this;
        }

        public ISqlBuilder WhereAll(string column, SqlQuery query)
        {
            this.query += $"WHERE {column} = ALL ({query.Query}) ";
            return this;
        }

        public ISqlBuilder WhereBetween(string column, object firstValue, object secondValue)
        {
            query += $"WHERE {column} BETWEEN {firstValue} AND {secondValue} ";
            return this;
        }

        public ISqlBuilder WhereLike(string column, string pattern)
        {
            query += $"WHERE {column} LIKE '{pattern}' ";
            return this;
        }

        public ISqlBuilder WhereNull(string column)
        {
            query += $"WHERE {column} IS NULL ";
            return this;
        }

        public ISqlBuilder WhereNotNull(string column)
        {
            query += $"WHERE {column} IS NOT NULL ";
            return this;
        }

        public ISqlBuilder Exists(SqlQuery query)
        {
            this.query += $"SELECT EXISTS({query.Query}) ";
            return this;
        }

        public ISqlBuilder WhereExists(SqlQuery query)
        {
            this.query += $"WHERE EXISTS ({query.Query}) ";
            return this;
        }

        public ISqlBuilder OrderBy(string column, OrderByType orderByType = OrderByType.Ascending)
        {
            query += $" ORDER BY {column} {(orderByType == OrderByType.Descending ? "DESC" : "ASC")} ";
            return this;
        }

        public ISqlBuilder OrderBy(SqlQuery query)
        {
            this.query += $" ORDER BY {query.Query}";
            return this;
        }

        public ISqlBuilder GroupBy(params string[] columns)
        {
            query += $"GROUP BY {string.Join(",", columns)} ";
            return this;
        }

        public ISqlBuilder Having(string clause = null)
        {
            query += $"HAVING {clause ?? string.Empty} ";
            return this;
        }

        public ISqlBuilder Join(string index, JoinIndex joinIndex, string joinTableAlias = "joinTable")
        {
            query += $"JOIN {joinIndex.Table} as {joinTableAlias} ON {index} = {joinTableAlias}.{joinIndex.Index} ";
            return this;
        }

        public ISqlBuilder LeftJoin(string index, JoinIndex joinIndex, string joinTableAlias = "joinTable")
        {
            query +=
                $"LEFT JOIN {joinIndex.Table} as {joinTableAlias} ON {index} = {joinTableAlias}.{joinIndex.Index} ";
            return this;
        }

        public ISqlBuilder RightJoin(string index, JoinIndex joinIndex, string joinTableAlias = "joinTable")
        {
            query +=
                $"RIGHT JOIN {joinIndex.Table} as {joinTableAlias} ON {index} = {joinTableAlias}.{joinIndex.Index} ";
            return this;
        }

        public ISqlBuilder Limit(int count)
        {
            query += $"LIMIT {count} ";
            return this;
        }

        public ISqlBuilder Pagination(int pageNumber, int pageSize)
        {
            query += $" LIMIT {(pageNumber - 1) * pageSize}, {pageSize} ";
            return this;
        }

        public ISqlBuilder Count(string clause)
        {
            query += $" COUNT({clause}) ";
            return this;
        }

        public ISqlBuilder Sum(string clause)
        {
            query += $" SUM({clause}) ";
            return this;
        }

        public ISqlBuilder As(string alias)
        {
            query += $" AS {alias} ";
            return this;
        }

        public ISqlBuilder If(object condition, string clause, string column)
        {
            query += $" IF({condition}, {clause}, {column}) ";
            return this;
        }

        public ISqlBuilder When(object clause)
        {
            query += $" WHEN {clause}";
            return this;
        }

        public ISqlBuilder Then(object clause)
        {
            query += $" THEN {clause}";
            return this;
        }

        public ISqlBuilder Like(string pattern = null)
        {
            query += $" LIKE {(pattern != null ? $"'{pattern}'" : string.Empty)}";
            return this;
        }

        public ISqlBuilder In<TValue>(TValue[] values)
        {
            var inValues = string.Empty;

            values.ToList().ForEach(v => inValues += $"'{v}',");
            inValues = inValues.RemoveLastCharacter();

            query += $" IN ({inValues}) ";

            return this;
        }

        public ISqlBuilder Concat(params string[] values)
        {
            query += " CONCAT(";

            values.ToList().ForEach(v => query += $"'{v}',");

            query = query.RemoveLastCharacter();
            query += ") ";

            return this;
        }

        public ISqlBuilder InsertInto(string table, ColumnValueDictionary columnValues)
        {
            query = $"INSERT INTO {table} ({string.Join(",", columnValues.Keys)})";
            query = query.RemoveLastCharacter();

            query += ") VALUES (";

            foreach (var key in columnValues.Keys)
                query += $"{(columnValues[key] != null ? $"'{columnValues[key]}'" : "NULL")},";

            query = query.RemoveLastCharacter();
            query += ")";

            return this;
        }

        public ISqlBuilder Update(string table, ColumnValueDictionary columnValues, ColumnValue idColumnValue)
        {
            query = $"UPDATE {table} SET ";

            foreach (var key in columnValues.Keys)
                query += $"{key}={(columnValues[key] != null ? $"'{columnValues[key]}'" : "NULL")},";

            query = query.RemoveLastCharacter();
            query += " ";

            Where(idColumnValue.Column).Equals.Append($"'{idColumnValue.Value}'");

            return this;
        }

        public ISqlBuilder Delete(string table, ColumnValue idColumnValue = null, bool where = true)
        {
            query = $"DELETE FROM {table} ";

            if (where && idColumnValue != null)
                Where($"`{idColumnValue.Column}`").Equals.Append($"'{idColumnValue.Value}'");

            return this;
        }

        public ISqlBuilder DeleteRange(string table, params ColumnValue[] idColumnValues)
        {
            if (!idColumnValues.Any()) return this;

            query = $"DELETE FROM {table} ";

            var columnName = idColumnValues.FirstOrDefault()?.Column;

            WhereIn($"`{columnName}`", idColumnValues.Select(cv => cv.Value).ToArray());

            return this;
        }

        public SqlQuery Build() => new($"{Regex.Replace(query, @"\s+", " ")}");

        #region private

        private static void AppendColumns<T>(ref string query, params string[] columnsToIgnore)
        {
            T instance = Activator.CreateInstance<T>();
            var instanceColumns = instance.GetPropertiesWithAttribute(typeof(ColumnAttribute))
                .Select(ca => ca.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name)
                .Where(c => !columnsToIgnore.Contains(c))
                .Select(c => $"`{c}`");

            query += string.Join(",", instanceColumns);
        }

        #endregion
    }
}