using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PhoneSmsRepository : Repository<PhoneSms>, IPhoneSmsRepository
    {
        public PhoneSmsRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<IEnumerable<PhoneSms>> GetPhoneSms(IMtaLogFiltersParams filters,
            IList<string> sourceAffectedValues)
        {
            sourceAffectedValues = sourceAffectedValues.Select(s => s.Replace("ph", string.Empty)).ToList();

            ISqlBuilder sourceValuesQuery = new SqlBuilder();
            ISqlBuilder affectedValuesQuery = new SqlBuilder();

            for (int i = 0; i < sourceAffectedValues.Count; i++)
            {
                if (i != 0)
                    (sourceValuesQuery, affectedValuesQuery) = (sourceValuesQuery.Or, affectedValuesQuery.Or);

                (sourceValuesQuery, affectedValuesQuery) = (
                    sourceValuesQuery.Append("p.from").Like().Concat("%", sourceAffectedValues[i], "%"),
                    affectedValuesQuery.Append("p.to").Like().Concat("%", sourceAffectedValues[i], "%"));
            }

            return await Query(new SqlBuilder()
                .Select()
                .From(Table).As("p")
                .Where("p.date").Between
                .Append($"'{DateTime.Now.AddHours(-(int) filters.MinTimeAgo).ToFullDate()}'")
                .And.Append($"'{DateTime.Now.AddHours(-(int) filters.MaxTimeAgo).ToFullDate()}'")
                .And.Case
                .When((int) filters.ContentFilterType).Equals.Append(0).Then("p.content").Like("%%")
                .When((int) filters.ContentFilterType).Equals.Append(1).Then("p.content").Equals
                .Append(string.IsNullOrEmpty(filters.Content) ? "''" : $"'{filters.Content}'")
                .When((int) filters.ContentFilterType).Equals.Append(2)
                .Then(new SqlBuilder().Append("p.content").Like().Concat("%", filters.Content, "%")
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
                    .Case.When((int) filters.SortType).Equals.Append(0).Then("p.date").End.Append(",")
                    .Case.When((int) filters.SortType).Equals.Append(1).Then("p.date").End.Append("DESC")
                    .Build())
                .Build()
            );
        }
    }
}