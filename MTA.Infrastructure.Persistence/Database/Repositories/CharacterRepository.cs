using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Extensions;
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
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        public CharacterRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<IEnumerable<Character>> GetCharactersByCharactername(string charactername)
            => await Query(new SqlBuilder()
                .Select("p.id, p.charactername")
                .From(Table).As("p")
                .WhereLike("p.charactername", $"%{charactername?.ToLower()}%")
                .OrderBy("p.charactername")
                .Limit(Constants.SearchFromDatabaseLimit)
                .Build());

        public async Task<IEnumerable<Character>> GetCharactersWithUserByCharactername(string charactername)
            => await QueryJoin<Character, User>(
                new SqlBuilder()
                    .Select("p.id", "p.charactername", "k.id", "k.username")
                    .From(Table).As("p")
                    .Join("p.account", new(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                    .GroupBy("p.id", "p.charactername", "k.id", "k.username")
                    .Having("p.charactername").Like($"%{charactername?.ToLower()}%")
                    .Build(), (character, user) =>
                {
                    character.SetUser(user);
                    return character;
                });

        public async Task<IEnumerable<Character>> GetCharactersByAdmin(IAdminCharacterFiltersParams request)
            => await Query(new SqlBuilder()
                .Select()
                .From(RepositoryDictionary.FindTable(typeof(ICharacterRepository))).As("p")
                .WhereBetween("p.lastlogin", $"'{request.MinLastLogin.ToFullDate()}'",
                    $"'{request.MaxLastLogin.ToFullDate()}'")
                .And.Append("p.creationdate").Between.Append($"'{request.MinCreationDate.ToFullDate()}'").And
                .Append($"'{request.MaxCreationDate.ToFullDate()}'")
                .And.If(new SqlBuilder().Append($"'{request.Name}'").IsNotNull.Build().Query,
                    new SqlBuilder().Append("p.charactername").Like($"%{request.Name}%").Build().Query,
                    "p.charactername")
                .And.Case
                .When($"{(int) request.GenderStatusType} = 0").Then("p.gender in (0, 1)")
                .When($"{(int) request.GenderStatusType} = 1").Then("p.gender = 0")
                .When($"{(int) request.GenderStatusType} = 2").Then("p.gender = 1")
                .End.And.Case
                .When($"{(int) request.ActiveStatusType} = 0").Then("p.gender in (0, 1)")
                .When($"{(int) request.ActiveStatusType} = 1").Then("p.gender = 1")
                .When($"{(int) request.ActiveStatusType} = 2").Then("p.gender = 0")
                .End.And.Case
                .When($"{(int) request.DeadStatusType} = 0").Then("p.cked in (0, 1)")
                .When($"{(int) request.DeadStatusType} = 1").Then("p.cked = 0")
                .When($"{(int) request.DeadStatusType} = 2").Then("p.cked = 1")
                .End
                .Append(CharacterSortTypeSmartEnum.FromValue((int) request.SortType).OrderBy().Query)
                .Build());

        public async Task<IEnumerable<Character>> GetAccountCharactersWithEstatesAndVehicles(int accountId)
        {
            var cacheCharacterDictionary = new Dictionary<int, Character>();
            var cacheEstateDictionary = new Dictionary<int, Estate>();
            var cacheVehicleDictionary = new Dictionary<int, Vehicle>();

            return (await QueryJoin<Character, Estate, Vehicle>(
                new SqlBuilder()
                    .Select()
                    .From(Table).As("p")
                    .LeftJoin("p.id", new(RepositoryDictionary.FindTable(typeof(IRepository<Estate>)), "owner"), "n")
                    .LeftJoin("p.id", new(RepositoryDictionary.FindTable(typeof(IRepository<Vehicle>)), "owner"), "poj")
                    .Where(new SqlBuilder().Append("p.account").Equals.Append(accountId).Build().Query)
                    .Build(), (character, estate, vehicle) =>
                {
                    Character characterEntry = default;
                    Estate estateEntry;
                    Vehicle vehicleEntry;

                    if (!cacheCharacterDictionary.TryGetValue(character.Id, out characterEntry))
                    {
                        characterEntry = character;
                        cacheCharacterDictionary.Add(characterEntry.Id, characterEntry);
                    }

                    if (estate != null && !cacheEstateDictionary.TryGetValue(estate.Id, out estateEntry))
                    {
                        estateEntry = estate;
                        characterEntry.Estates.Add(estateEntry);

                        cacheEstateDictionary.Add(estateEntry.Id, estateEntry);
                    }

                    if (vehicle != null && !cacheVehicleDictionary.TryGetValue(vehicle.Id, out vehicleEntry))
                    {
                        vehicleEntry = vehicle;
                        characterEntry.Vehicles.Add(vehicleEntry);

                        cacheVehicleDictionary.Add(vehicleEntry.Id, vehicleEntry);
                    }

                    return characterEntry;
                }
            ));
        }

        public async Task<IEnumerable<Character>> GetMostActiveCharacters(int top = Constants.TopStatsLimit)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("lastlogin")
                .GreaterEquals
                .Append($"DATE_ADD(NOW(), INTERVAL {-Constants.MaximumInactiveDays} DAY)")
                .OrderBy("hoursplayed", OrderByType.Descending)
                .Limit(top)
                .Build());

        public async Task<IEnumerable<Character>> GetRichestCharacters(int top = Constants.TopStatsLimit)
            => await Query(new SqlBuilder()
                .Select(false)
                .Append("p.charactername").Append(",")
                .Open
                .Append("IFNULL(p.money, 0)").Add
                .Append("IFNULL(p.bankmoney, 0)").Add
                .Append("IFNULL").Append(new SqlBuilder()
                    .Open.Open
                    .Select(false)
                    .Sum("n.cost").From(RepositoryDictionary.FindTable(typeof(IRepository<Estate>))).As("n")
                    .Where("n.owner").Equals.Append("p.id")
                    .Close
                    .Append(", 0").Close
                    .Build().Query).Add
                .Append("IFNULL").Append(new SqlBuilder()
                    .Open.Open
                    .Select(false)
                    .Sum("pojs.vehprice").From(RepositoryDictionary.FindTable(typeof(IRepository<VehiclesShop>)))
                    .As("pojs")
                    .Where("pojs.vehmtamodel").Append("IN").Open
                    .Append(new SqlBuilder()
                        .Select(false)
                        .Append("poj.model")
                        .From(RepositoryDictionary.FindTable(typeof(IRepository<Vehicle>))).As("poj")
                        .Where("poj.owner").Equals.Append("p.id")
                        .Build().Query
                    )
                    .Close
                    .Close
                    .Append(", 0")
                    .Close
                    .Build().Query)
                .Close.As("TotalMoney")
                .From(Table).As("p")
                .Where("lastlogin").GreaterEquals
                .Append($"DATE_ADD(NOW(), INTERVAL {-Constants.MaximumInactiveDays} DAY)")
                .OrderBy("TotalMoney", OrderByType.Descending)
                .Limit(top)
                .Build());
    }
}