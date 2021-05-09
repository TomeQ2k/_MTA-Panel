using System.Collections.Generic;
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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<IEnumerable<Order>> GetOrders(IOrderFiltersParams filtersParams)
            => await QueryJoin<Order, PremiumFile, User>(new SqlBuilder()
                .Select()
                .From(Table).As("o")
                .LeftJoin("o.id", new(RepositoryDictionary.FindTable(typeof(IPremiumFileRepository)), "orderId"), "f")
                .LeftJoin("o.userId", new(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                .Where(new SqlBuilder().If(
                    new SqlBuilder().Append($"'{filtersParams.Username}'").IsNotNull.Build().Query,
                    new SqlBuilder().Append("k.username").Like($"%{filtersParams.Username}%").Build()
                        .Query, "k.username").Build().Query)
                .And.Case
                .When($"{(int) filtersParams.StateStatusType} = -1").Then("o.approvalState in (0,1,2)")
                .When($"{(int) filtersParams.StateStatusType} = 0").Then("o.approvalState = 0")
                .When($"{(int) filtersParams.StateStatusType} = 1").Then("o.approvalState = 1")
                .When($"{(int) filtersParams.StateStatusType} = 2").Then("o.approvalState = 2")
                .End.OrderBy("o.dateCreated", (OrderByType) filtersParams.SortType)
                .Build(), (order, orderFile, user) =>
            {
                order.SetOrderFile(orderFile);
                order.SetUser(user);

                return order;
            });
    }
}