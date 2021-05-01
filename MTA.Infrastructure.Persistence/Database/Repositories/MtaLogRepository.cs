using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Application.Extensions;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class MtaLogRepository : Repository<MtaLog>, IMtaLogRepository
    {
        public MtaLogRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<IEnumerable<MtaLog>> GetMtaLogs(IMtaLogFiltersParams filters,
            IList<string> sourceAffectedValues)
        {
            ISqlBuilder sourceValuesQuery = new SqlBuilder();
            ISqlBuilder affectedValuesQuery = new SqlBuilder();

            for (int i = 0; i < sourceAffectedValues.Count; i++)
            {
                if (i != 0)
                    (sourceValuesQuery, affectedValuesQuery) = (sourceValuesQuery.Or, affectedValuesQuery.Or);

                (sourceValuesQuery, affectedValuesQuery) = (
                    sourceValuesQuery.Append("source").Like().Concat("%", sourceAffectedValues[i], "%"),
                    affectedValuesQuery.Append("affected").Like().Concat("%", sourceAffectedValues[i], "%"));
            }

            return await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .WhereIn("action", Array.ConvertAll(filters.Actions, action => (int) action))
                .And.Append("time").Between
                .Append($"'{DateTime.Now.AddHours(-(int) filters.MinTimeAgo).ToFullDate()}'")
                .And.Append($"'{DateTime.Now.AddHours(-(int) filters.MaxTimeAgo).ToFullDate()}'")
                .And.Case
                .When((int) filters.ContentFilterType).Equals.Append(0)
                .Then(new SqlBuilder().Append("content").Like("%%").Build().Query)
                .When((int) filters.ContentFilterType).Equals.Append(1).Then("content").Equals
                .Append(string.IsNullOrEmpty(filters.Content) ? "''" : $"'{filters.Content}'")
                .When((int) filters.ContentFilterType).Equals.Append(2)
                .Then(new SqlBuilder().Append("content").Like().Concat("%", filters.Content, "%")
                    .Build().Query)
                .End
                .And.Case
                .When((int) filters.SourceAffectedFilterType).Equals.Append(0)
                .Then(sourceValuesQuery.Build().Query)
                .Or.Append(affectedValuesQuery.Build().Query)
                .When((int) filters.SourceAffectedFilterType).Equals.Append(1)
                .Then(sourceValuesQuery.Build().Query)
                .When((int) filters.SourceAffectedFilterType).Equals.Append(2)
                .Then(affectedValuesQuery.Build().Query)
                .End
                .OrderBy(new SqlBuilder()
                    .Case.When((int) filters.SortType).Equals.Append(0).Then("time").End.Append(",")
                    .Case.When((int) filters.SortType).Equals.Append(1).Then("time").End.Append("DESC")
                    .Build())
                .Build()
            );
        }
    }
}