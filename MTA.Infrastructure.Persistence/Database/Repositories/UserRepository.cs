using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<User> FindUserById(int userId)
            => await QueryFirst(new SqlBuilder()
                .Select().Append(",").Append("b.id").As("BanId")
                .From(Table).As("k")
                .LeftJoin("k.id", new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<Ban>)), "account"),
                    "b")
                .Where("k.id")
                .Equals
                .Append(userId)
                .Build());

        public async Task<User> FindUserByEmail(string email)
            => await QueryFirst(new SqlBuilder()
                .Select().Append(",").Append("b.id").As("BanId")
                .From(Table).As("k")
                .LeftJoin("k.id", new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<Ban>)), "account"),
                    "b")
                .Where("k.email")
                .Equals
                .Append($"'{email}'")
                .Build());

        public async Task<User> FindUserByUsername(string username)
            => await QueryFirst(new SqlBuilder()
                .Select().Append(",").Append("b.id").As("BanId")
                .From(Table).As("k")
                .LeftJoin("k.id", new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<Ban>)), "account"),
                    "b")
                .Where("k.username")
                .Equals
                .Append($"'{username}'")
                .Build());

        public async Task<User> FindUserWithSerials(int userId)
        {
            var userCacheDictionary = new Dictionary<int, User>();
            var serialCacheDictionary = new Dictionary<int, Serial>();

            return (await QueryJoin<User, Serial>(new SqlBuilder()
                .Select()
                .From(Table).As("u")
                .LeftJoin("u.id", new JoinIndex(RepositoryDictionary.FindTable(typeof(ISerialRepository)), "userid"),
                    "s")
                .Where("k.id").Equals.Append(userId)
                .Build(), (user, serial) =>
            {
                User userEntry;
                Serial serialEntry;

                if (!userCacheDictionary.TryGetValue(user.Id, out userEntry))
                {
                    userEntry = user;
                    userCacheDictionary.Add(userEntry.Id, userEntry);
                }

                if (serial != null && !serialCacheDictionary.TryGetValue(serial.Id, out serialEntry))
                {
                    serialEntry = serial;
                    userEntry.Serials.Add(serialEntry);

                    serialCacheDictionary.Add(serialEntry.Id, serialEntry);
                }

                return userEntry;
            })).FirstOrDefault();
        }

        public async Task<User> GetUserByEmailWithTokenType(string email, TokenType tokenType)
            => (await QueryJoin<User, Token>(new SqlBuilder()
                .Select()
                .From(Table).As("k")
                .Join("k.id", new JoinIndex("tokens", "userId"), "t")
                .Where("email")
                .Equals
                .Append($"'{email}'")
                .And
                .Append("t.tokenType")
                .Equals
                .Append($"{(int) tokenType}")
                .Build(), (user, token) =>
            {
                user.SetToken(token);
                return user;
            })).FirstOrDefault();

        public async Task<User> GetUserWithCharacters(int userId)
        {
            var cacheUserDictionary = new Dictionary<int, User>();
            var cacheCharacterDictionary = new Dictionary<int, Character>();
            var cacheEstateDictionary = new Dictionary<int, Estate>();
            var cacheVehicleDictionary = new Dictionary<int, Vehicle>();
            var cacheSerialDictionary = new Dictionary<int, Serial>();

            return (await QueryJoin<User, Serial, Character, Faction, Vehicle, VehiclesShop, Estate>(
                new SqlBuilder()
                    .Select()
                    .From(Table).As("k")
                    .LeftJoin("k.id",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(ISerialRepository)), "userid"), "s")
                    .LeftJoin("k.id",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(ICharacterRepository)),
                            "account"),
                        "p")
                    .LeftJoin("p.faction_id",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(IFactionRepository)), "id"),
                        "f")
                    .LeftJoin("p.id",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<Vehicle>)),
                            "owner"),
                        "pj")
                    .LeftJoin("pj.model",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<VehiclesShop>)), "vehmtamodel"),
                        "ps")
                    .LeftJoin("p.id",
                        new JoinIndex(RepositoryDictionary.FindTable(typeof(IRepository<Estate>)), "owner"), "n")
                    .Where("k.id")
                    .Equals
                    .Append(userId)
                    .Build(), (user, serial, character, faction, vehicle, vehicleShop, estate) =>
                {
                    User userEntry;
                    Character characterEntry = default;
                    Vehicle vehicleEntry;
                    Estate estateEntry;
                    Serial serialEntry;

                    if (!cacheUserDictionary.TryGetValue(user.Id, out userEntry))
                    {
                        userEntry = user;
                        cacheUserDictionary.Add(userEntry.Id, userEntry);
                    }

                    if (serial != null && !cacheSerialDictionary.TryGetValue(serial.Id, out serialEntry))
                    {
                        serialEntry = serial;
                        userEntry.Serials.Add(serialEntry);

                        cacheSerialDictionary.Add(serialEntry.Id, serialEntry);
                    }

                    if (character != null &&
                        !cacheCharacterDictionary.TryGetValue(character.Id, out characterEntry))
                    {
                        characterEntry = character;
                        characterEntry.SetFaction(faction);
                        
                        cacheCharacterDictionary.Add(characterEntry.Id, characterEntry);
                    }

                    if (estate != null && character != null &&
                        !cacheEstateDictionary.TryGetValue(estate.Id, out estateEntry))
                    {
                        estateEntry = estate;
                        characterEntry.Estates.Add(estateEntry);

                        cacheEstateDictionary.Add(estateEntry.Id, estateEntry);
                    }

                    if (vehicle != null && character != null &&
                        !cacheVehicleDictionary.TryGetValue(vehicle.Id, out vehicleEntry))
                    {
                        vehicleEntry = vehicle;
                        vehicleEntry.SetVehiclesShop(vehicleShop);
                        characterEntry.Vehicles.Add(vehicleEntry);

                        cacheVehicleDictionary.Add(vehicleEntry.Id, vehicleEntry);
                    }

                    if (characterEntry != null)
                        userEntry.Characters.Add(characterEntry);

                    return userEntry;
                }
            )).FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetUsersByUsername(string username)
            => await Query(new SqlBuilder()
                .Select("k.id", "k.username")
                .From(Table).As("k")
                .WhereLike("k.username", $"%{username?.ToLower()}%")
                .OrderBy("k.username")
                .Limit(Constants.SearchFromDatabaseLimit)
                .Build());

        public async Task<IEnumerable<User>> GetUsersByAdmin(IAdminUserFiltersParams request)
            => await Query(new SqlBuilder()
                .Select().Append(",").Append("b.id").As("BanId")
                .From(Table).As("k")
                .LeftJoin("k.id", new("bans", "account"), "b")
                .WhereBetween("k.registerdate", $"'{request.MinRegisterDate.ToFullDate()}'",
                    $"'{request.MaxRegisterDate.ToFullDate()}'")
                .And.Append("k.lastlogin").Between.Append($"'{request.MinLastLogin.ToFullDate()}'").And
                .Append($"'{request.MaxLastLogin.ToFullDate()}'")
                .And.Case
                .When($"{(int) request.ActivatedStatusType} = 0").Then("k.activated in (0, 1)")
                .When($"{(int) request.ActivatedStatusType} = 1").Then("k.activated = 1")
                .When($"{(int) request.ActivatedStatusType} = 2").Then("k.activated = 0")
                .End.And.Case
                .When($"{(int) request.AppStateType} = 0").Then("k.appstate in (0, 1, 2, 3)")
                .When($"{(int) request.AppStateType} = 1").Then("k.appstate = 1")
                .When($"{(int) request.AppStateType} = 2").Then("k.appstate = 2")
                .When($"{(int) request.AppStateType} = 3").Then("k.appstate = 3")
                .When($"{(int) request.AppStateType} = 4").Then("k.appstate = 0")
                .End
                .And.Case
                .When($"{(int) request.BanStatusType} = 0").Then("b.id or b.id").IsNull
                .When($"{(int) request.BanStatusType} = 1").Then("b.id")
                .When($"{(int) request.BanStatusType} = 2").Then("b.id").IsNull
                .End
                .And.If(new SqlBuilder().Append($"'{request.Ip}'").IsNotNull.Build().Query,
                    new SqlBuilder().Append("k.ip").Like($"%{request.Ip}%").Build().Query, "k.ip")
                .And.If(
                    new SqlBuilder().Append($"'{request.Serial.ToUpperInvariant()}'").IsNotNull.Build()
                        .Query,
                    new SqlBuilder().Append("k.mtaserial").Like("%{request.Serial}%").Build().Query,
                    "k.mtaserial")
                .Append(UserSortTypeSmartEnum.FromValue((int) request.SortType).OrderBy().Query)
                .Build());

        public async Task<IEnumerable<User>> GetUsersWithAssignedReports(ReportCategoryType reportCategoryType,
            bool isPrivate = false)
        {
            var cacheUserDictionary = new Dictionary<int, User>();
            var cacheReportDictionary = new Dictionary<string, Report>();

            return (await QueryJoin<User, Report>(new SqlBuilder()
                    .Select()
                    .From(Table).As("k")
                    .LeftJoin("k.id", new(RepositoryDictionary.FindTable(typeof(IReportRepository)), "assigneeId"), "r")
                    .Append(ReportCategoryTypeSmartEnum.FromValue((int) reportCategoryType)
                        .WhereReportCategoryRolesAre(isPrivate).Query)
                    .Build(), (user, report) =>
                {
                    User userEntry;
                    Report reportEntry;

                    if (!cacheUserDictionary.TryGetValue(user.Id, out userEntry))
                    {
                        userEntry = user;
                        cacheUserDictionary.Add(userEntry.Id, userEntry);
                    }

                    if (report != null && !cacheReportDictionary.TryGetValue(report.Id, out reportEntry))
                    {
                        reportEntry = report;
                        userEntry.AssignedReports.Add(reportEntry);

                        cacheReportDictionary.Add(reportEntry.Id, reportEntry);
                    }

                    return userEntry;
                }))
                .GroupBy(u => u.Id)
                .Select(g => g.OrderByDescending(u => u.AssignedReports.Count).First());
        }

        public async Task<IEnumerable<User>> GetMostActiveAdmins(int top = Constants.TopStatsLimit)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("admin").Add.Append("supporter").Add.Append("vct").Add.Append("mapper").Add.Append("scripter")
                .Greater
                .Append(0)
                .And.Append("lastlogin").GreaterEquals
                .Append($"DATE_ADD(NOW(), INTERVAL {-Constants.MaximumInactiveDays} DAY)")
                .OrderBy("adminreports", OrderByType.Descending)
                .Limit(top)
                .Build());

        public async Task<bool> SerialExists(string serial)
            => (await SelectQueryFirst<SerialExistsResult>(
                new SqlBuilder()
                    .Exists(new SqlBuilder()
                        .Select()
                        .From(Table)
                        .Where("mtaserial")
                        .Equals
                        .Append($"'{serial}'")
                        .Build())
                    .As(nameof(SerialExists))
                    .Build())).SerialExists;
    }
}