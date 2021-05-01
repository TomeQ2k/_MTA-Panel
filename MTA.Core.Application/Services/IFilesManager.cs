using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Services
{
    public interface IFilesManager : IReadOnlyFilesManager
    {
        Task<FileModel> Upload(IFormFile file, string filePath = null);
        Task<IList<FileModel>> Upload(IEnumerable<IFormFile> files, string filePath = null);

        void Delete(string path);
        void DeleteDirectory(string path, bool isRecursive = true);
        void DeleteByFullPath(string fullPath);

        Task WriteFile(string file, string filePath);

        Task ReplaceInFile(string filePath, string oldValue, string newValue);
    }
}