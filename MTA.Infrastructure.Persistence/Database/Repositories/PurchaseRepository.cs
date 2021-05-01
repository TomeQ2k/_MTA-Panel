using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesWithUsers(DateSortType sortType,
            int userId)
            => await QueryJoin<Purchase, User>(
                new SqlBuilder()
                    .Select("p.id", "p.name", "p.cost", "p.date", "p.account", "k.id", "k.username")
                    .From(Table)
                    .As("p")
                    .LeftJoin("p.account",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                    .Where("p.account").Equals.Append(userId)
                    .OrderBy("p.date", (OrderByType) sortType)
                    .Build(), (purchase, user) =>
                {
                    purchase.SetAccount(user);
                    return purchase;
                }
            );

        public async Task<IEnumerable<Purchase>> GetPurchasesWithUsersByAdmin(IAdminPurchaseFiltersParams filtersParams)
        {
            var query = new SqlBuilder()
                .Select("p.id", "p.name", "p.cost", "p.date", "p.account", "k.id", "k.username")
                .From(Table)
                .As("p")
                .LeftJoin("p.account",
                    new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                .Where(new SqlBuilder().If(
                    new SqlBuilder().Append($"'{filtersParams.Username}'").IsNotNull.Build().Query,
                    new SqlBuilder().Append("k.username").Like($"%{filtersParams.Username}%").Build()
                        .Query, "k.username").Build().Query)
                .OrderBy("p.date", (OrderByType) filtersParams.SortType)
                .Build().Query;
            
            return await QueryJoin<Purchase, User>(
                new SqlBuilder()
                    .Select("p.id", "p.name", "p.cost", "p.date", "p.account", "k.id", "k.username")
                    .From(Table)
                    .As("p")
                    .LeftJoin("p.account",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                    .Where(new SqlBuilder().If(
                        new SqlBuilder().Append($"'{filtersParams.Username}'").IsNotNull.Build().Query,
                        new SqlBuilder().Append("k.username").Like($"%{filtersParams.Username}%").Build()
                            .Query, "k.username").Build().Query)
                    .OrderBy("p.date", (OrderByType) filtersParams.SortType)
                    .Build(), (purchase, user) =>
                {
                    purchase.SetAccount(user);
                    return purchase;
                }
            );
        }
    }
}