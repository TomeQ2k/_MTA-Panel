using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class PremiumFileRepository : Repository<PremiumFile>, IPremiumFileRepository
    {
        public PremiumFileRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<PremiumFile> GetFileWithOrder(string premiumFileId)
            => (await QueryJoin<PremiumFile, Order>(new SqlBuilder()
                .Select()
                .From(Table).As("pf")
                .Join("pf.orderId", new JoinIndex(RepositoryDictionary.FindTable(typeof(IOrderRepository)), "id"),
                    "o")
                .Where("pf.id").Equals.Append($"'{premiumFileId}'")
                .Build(), (file, order) =>
            {
                file.SetOrder(order);
                return file;
            })).FirstOrDefault();


        public async Task<PremiumFile> GetFileWithOrderAndEstate(string premiumFileId)
            => (await QueryJoin<PremiumFile, Order, Estate>(new SqlBuilder()
                .Select()
                .From(Table).As("pf")
                .Join("pf.orderId", new JoinIndex(RepositoryDictionary.FindTable(typeof(IOrderRepository)), "id"),
                    "o")
                .Join("o.estateId", new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<Estate>)), "id"),
                    "e")
                .Where("pf.id").Equals.Append($"'{premiumFileId}'")
                .Build(), (file, order, estate) =>
            {
                file.SetOrder(order);
                file.SetEstate(estate);
                return file;
            })).FirstOrDefault();
    }
}