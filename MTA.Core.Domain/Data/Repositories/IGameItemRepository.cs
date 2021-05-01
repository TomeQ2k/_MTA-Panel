using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IGameItemRepository : IRepository<GameItem>
    {
        Task<IEnumerable<GameItem>> GetCharacterItems(int characterId);
        Task<IEnumerable<GameItem>> GetAccountItems(IEnumerable<int> charactersIds);
    }
}