using System.Threading.Tasks;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services
{
    public interface ICharacterManager
    {
        Task<BlockCharacterResult> ToggleBlockCharacter(int characterId);
    }
}