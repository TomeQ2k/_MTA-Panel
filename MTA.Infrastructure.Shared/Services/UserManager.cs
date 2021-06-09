using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class UserManager : IUserManager
    {
        private readonly IDatabase database;
        private readonly IBanService banService;
        private readonly IReadOnlySerialService serialService;
        private readonly IRolesService rolesService;
        private readonly IHttpContextReader httpContextReader;

        public UserManager(IDatabase database, IBanService banService, IReadOnlySerialService serialService,
            IRolesService rolesService, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.banService = banService;
            this.serialService = serialService;
            this.rolesService = rolesService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<bool> BlockAccount(BlockAccountRequest request)
        {
            if (!await rolesService.IsPermitted(httpContextReader.CurrentUserId, Constants.AllOwnersRoles))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var bansToInsert = InitBans(request).ToList();

            return await InsertBansInTransaction(bansToInsert, request.AccountId)
                ? true
                : throw new DatabaseException(ErrorMessages.DatabaseTransactionErrorMessage);
        }

        public async Task<AddCreditsResult> AddCredits(int credits, int userId)
        {
            var user = await database.UserRepository.FindById(userId) ??
                       throw new EntityNotFoundException("User not found");

            user.AddCredits(credits);

            return await database.UserRepository.Update(user)
                ? new AddCreditsResult(user.Credits)
                : throw new DatabaseException();
        }

        public async Task<bool> CleanAccount(int userId)
        {
            var user = await database.UserRepository.GetUserWithCharacters(userId)
                       ?? throw new EntityNotFoundException("User not found");

            var (characters, estates, vehicles) =
                (user.Characters, user.Characters.SelectMany(c => c.Estates),
                    user.Characters.SelectMany(c => c.Vehicles));

            var gameItems = await database.GameItemRepository.GetAccountItems(characters.Select(c => c.Id));

            using (var transaction = database.BeginTransaction().Transaction)
            {
                await ExecuteCleanAccount(characters, estates, vehicles, gameItems);

                transaction.Complete();
            }

            return true;
        }

        #region private

        private IEnumerable<Ban> InitBans(BlockAccountRequest request)
            => request.Serials.Select(serial => new BanBuilder()
                    .BanAccount(request.AccountId)
                    .BanSerial(serial)
                    .ByAdmin(httpContextReader.CurrentUserId)
                    .SetReason(request.Reason)
                    .Build())
                .Concat(request.Ips.Select(ip => new BanBuilder()
                    .BanAccount(request.AccountId)
                    .BanIp(ip)
                    .ByAdmin(httpContextReader.CurrentUserId)
                    .SetReason(request.Reason)
                    .Build()));

        private async Task<bool> InsertBansInTransaction(List<Ban> bansToInsert, int accountId)
        {
            bool isBlocked = true;

            using (var transaction = database.BeginTransaction().Transaction)
            {
                foreach (var b in bansToInsert)
                {
                    if (b.Serial != null && !await serialService.SerialExists(b.Serial, accountId))
                        throw new ServerException("User serial does not exist in database");

                    isBlocked = isBlocked && await banService.AddBan(b) != null;
                }

                transaction.Complete();
            }

            return isBlocked;
        }

        private async Task ExecuteCleanAccount(ICollection<Character> characters, IEnumerable<Estate> estates,
            IEnumerable<Vehicle> vehicles,
            IEnumerable<GameItem> gameItems)
        {
            if (!await database.CharacterRepository.DeleteRange(characters))
                throw new DatabaseException();

            if (!await database.EstateRepository.DeleteRange(estates))
                throw new DatabaseException();

            if (!await database.VehicleRepository.DeleteRange(vehicles))
                throw new DatabaseException();

            if (!await database.GameItemRepository.DeleteRange(gameItems))
                throw new DatabaseException();
        }

        #endregion
    }
}