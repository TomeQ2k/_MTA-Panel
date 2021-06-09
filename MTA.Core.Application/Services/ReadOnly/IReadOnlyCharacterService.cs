using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Models;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyCharacterService
    {
        Task<Character> GetCharacter(int characterId);

        Task<IEnumerable<Character>> GetCharactersByCharactername(string charactername);
        Task<IEnumerable<Character>> GetCharactersWithUserByCharactername(string charactername);
        Task<PagedList<Character>> GetCharactersByAdmin(GetCharactersByAdminRequest request);
        Task<IEnumerable<Character>> GetAccountCharactersWithEstatesAndVehicles();

        Task<Estate> HasAnyCharacterEstate(IEnumerable<Character> characters, int estateId);
    }
}