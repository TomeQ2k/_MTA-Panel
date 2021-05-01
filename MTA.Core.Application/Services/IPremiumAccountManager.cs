using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services
{
    public interface IPremiumAccountManager
    {
        Task<ObjectProtectionResult> AddObjectProtection(AddObjectProtectionRequest request);
        Task<AddSerialSlotResult> AddSerialSlot(AddSerialSlotRequest request);
        Task<bool> AddCustomSkin(IFormFile skinFile, int skinId, int characterId);
        Task<bool> AddCustomInterior(IFormFile interiorFile, int estateId);
        Task<bool> TransferCharacter(int sourceCharacterId, int targetCharacterId);
    }
}