using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IPremiumUserLibraryManager : IReadOnlyPremiumUserLibraryManager
    {
        Task<PremiumFile> AddFileToLibrary(IFormFile file, PremiumFileType type, string orderId, int? skinId = null);
        Task<bool> ChangeUploadedSkinFile(IFormFile newFile, string oldFileId, int? skinId = null);
        Task<bool> ChangeUploadedInteriorFile(IFormFile newFile, string oldFileId);
    }
}