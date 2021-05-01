using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface ICharacterService : IReadOnlyCharacterService
    {
        Task<bool> TransferMoney(Character sourceCharacter, Character targetCharacter);

        Task<bool> TransferEstatesAndVehicles(IEnumerable<Estate> estates, IEnumerable<Vehicle> vehicles,
            int targetCharacterId);

        Task<bool> TransferGameItems(Character sourceCharacter, Character targetCharacter);
    }
}