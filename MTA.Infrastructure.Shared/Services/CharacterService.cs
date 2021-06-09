using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public CharacterService(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<Character> GetCharacter(int characterId)
            => await database.CharacterRepository.FindById(characterId);

        public async Task<IEnumerable<Character>> GetCharactersByCharactername(string charactername)
            => await database.CharacterRepository.GetCharactersByCharactername(charactername);

        public async Task<IEnumerable<Character>> GetCharactersWithUserByCharactername(string charactername)
            => await database.CharacterRepository.GetCharactersWithUserByCharactername(charactername);

        public async Task<PagedList<Character>> GetCharactersByAdmin(GetCharactersByAdminRequest request)
            => (await database.CharacterRepository.GetCharactersByAdmin(request)).ToPagedList(request.PageNumber,
                request.PageSize);

        public async Task<IEnumerable<Character>> GetAccountCharactersWithEstatesAndVehicles()
            => await database.CharacterRepository.GetAccountCharactersWithEstatesAndVehicles(httpContextReader
                .CurrentUserId);

        public async Task<bool> TransferMoney(Character sourceCharacter, Character targetCharacter)
        {
            if (sourceCharacter == null || targetCharacter == null)
                throw new EntityNotFoundException("Some of characters not found");

            targetCharacter.AddMoney(sourceCharacter.Money, sourceCharacter.BankMoney);
            sourceCharacter.AddMoney(-sourceCharacter.Money, -sourceCharacter.BankMoney);

            return await database.CharacterRepository.UpdateRange(
                new List<Character> {sourceCharacter, targetCharacter});
        }

        public async Task<bool> TransferEstatesAndVehicles(IEnumerable<Estate> estates, IEnumerable<Vehicle> vehicles,
            int targetCharacterId)
        {
            estates.ToList().ForEach(e => e.SetOwnerId(targetCharacterId));
            vehicles.ToList().ForEach(v => v.SetOwnerId(targetCharacterId));

            return await database.EstateRepository.UpdateRange(estates) &&
                   await database.VehicleRepository.UpdateRange(vehicles);
        }

        public async Task<bool> TransferGameItems(Character sourceCharacter, Character targetCharacter)
        {
            var sourceCharacterItems = await database.GameItemRepository.GetCharacterItems(sourceCharacter.Id);

            sourceCharacterItems.ToList().ForEach(i => i.SetOwnerId(targetCharacter.Id));

            return await database.GameItemRepository.UpdateRange(sourceCharacterItems);
        }

        public async Task<Estate> HasAnyCharacterEstate(IEnumerable<Character> characters, int estateId)
        {
            foreach (var character in characters)
            {
                var estate = character.Estates.Where(e => e.Id == estateId).FirstOrDefault();
               
                if(estate == null)
                    estate = await IsCharacterOwnerOfFactionEstate(character, estateId);

                if (estate != null && estate.Id == estateId)
                    return estate;
            }

            return null;
        }

        #region private

        private async Task<Estate> IsCharacterOwnerOfFactionEstate(Character character, int estateId)
            => character.IsFactionLeader
                ? await database.EstateRepository.FindByColumn(new("faction", character.FactionId))
                : null;

        #endregion
    }
}