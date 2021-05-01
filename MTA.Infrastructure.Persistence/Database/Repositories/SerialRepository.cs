using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class SerialRepository : Repository<Serial>, ISerialRepository
    {
        public SerialRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<IEnumerable<Serial>> GetUserSerials(int userId)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("userId").Equals.Append(userId)
                .And.Append("last_login_date").IsNotNull
                .And.Append("status").Equals.Append(1)
                .OrderBy("last_login_date", OrderByType.Descending)
                .Build());

        public async Task<bool> DeleteSerial(int serialId, int userId)
            => await Execute(new SqlBuilder()
                .Delete(Table, new ColumnValue("id", serialId))
                .And.Append("userid")
                .Equals.Append(userId)
                .Build());

        public async Task<bool> SerialExists(string serial, int userId)
            => (await SelectQueryFirst<SerialExistsResult>(
                new SqlBuilder()
                    .Exists(new SqlBuilder()
                        .Select()
                        .From(Table)
                        .Where("userid")
                        .Equals
                        .Append(userId)
                        .And
                        .Append("serial")
                        .Equals
                        .Append($"'{serial}'")
                        .Build())
                    .As(nameof(SerialExists))
                    .Build())).SerialExists;
    }
}