using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Task<IEnumerable<Character>> GetCharactersByCharactername(string charactername);
        Task<IEnumerable<Character>> GetCharactersWithUserByCharactername(string charactername);
        Task<IEnumerable<Character>> GetCharactersByAdmin(IAdminCharacterFiltersParams request);
        Task<IEnumerable<Character>> GetAccountCharactersWithEstatesAndVehicles(int accountId);
        Task<IEnumerable<Character>> GetMostActiveCharacters(int top = Constants.TopStatsLimit);
        Task<IEnumerable<Character>> GetRichestCharacters(int top = Constants.TopStatsLimit);
    }
}