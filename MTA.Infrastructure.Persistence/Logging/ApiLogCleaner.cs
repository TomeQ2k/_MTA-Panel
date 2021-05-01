using System;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;

namespace MTA.Infrastructure.Persistence.Logging
{
    public class ApiLogCleaner : ILogCleaner
    {
        private readonly IFilesManager filesManager;

        private const string LogsFilesPath = "./wwwroot/logs/";
        private const int LogsFilePathLength = 31;

        public ApiLogCleaner(IFilesManager filesManager)
        {
            this.filesManager = filesManager;
        }

        public Task<bool> ClearLogs()
        {
            var logFilesPathsToDelete = filesManager.GetDirectoryFilesNames(LogsFilesPath)
                .Where(l => l.Length == LogsFilePathLength &&
                            CreateLogFileDateTime(l).AddDays(Constants.ApiLogTrashTimeInDays) < DateTime.Now);

            logFilesPathsToDelete.ToList().ForEach(l => filesManager.DeleteByFullPath(l));

            return Task.FromResult(true);
        }

        #region private

        private static DateTime CreateLogFileDateTime(string logFilePath)
            => new DateTime(int.Parse(logFilePath.Substring(logFilePath.Length - 12, 4)),
                int.Parse(logFilePath.Substring(logFilePath.Length - 8, 2)),
                int.Parse(logFilePath.Substring(logFilePath.Length - 6, 2)));

        #endregion
    }
}