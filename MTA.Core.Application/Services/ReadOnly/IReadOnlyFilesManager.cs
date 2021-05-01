using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyFilesManager
    {
        string ProjectPath { get; }
        string WebRootPath { get; }

        Task<string> ReadFile(string filePath);
        Task<string[]> ReadFileLines(string filePath);

        bool FileExists(string filePath);

        IEnumerable<string> GetDirectoryFilesNames(string path);
    }
}