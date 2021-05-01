using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Logging
{
    public class LogReader : ILogReader
    {
        private readonly IDatabase database;
        private readonly IReadOnlyFilesManager filesManager;
        private readonly IMapper mapper;
        private readonly IHttpContextWriter httpContextWriter;
        private readonly LogKeyWordsDictionary logKeyWordsDictionary;

        public LogReader(IDatabase database, IReadOnlyFilesManager filesManager, IMapper mapper,
            IHttpContextWriter httpContextWriter, LogKeyWordsDictionary logKeyWordsDictionary)
        {
            this.database = database;
            this.filesManager = filesManager;
            this.mapper = mapper;
            this.httpContextWriter = httpContextWriter;
            this.logKeyWordsDictionary = logKeyWordsDictionary;
        }

        public async Task<IEnumerable<MtaLogModel>> GetMtaLogsFromDatabase(MtaLogFiltersParams filters,
            IEnumerable<SourceAffectedModel> sourceAffectedModels)
        {
            var sourceAffectedValues = FetchSourceAffectedValues(filters, sourceAffectedModels);

            PagedList<MtaLog> mtaLogs = default;
            PagedList<PhoneSms> phoneSms = default;

            if (filters.Actions.Any(l => l != LogAction.PhoneSms))
                mtaLogs = (await database.MtaLogRepository.GetMtaLogs(filters, sourceAffectedValues))
                    .ToPagedList(filters.PageNumber, filters.PageSize);

            if (filters.Actions.Contains(LogAction.PhoneSms))
                phoneSms = (await database.PhoneSmsRepository.GetPhoneSms(filters, sourceAffectedValues))
                    .ToPagedList(filters.PageNumber, filters.PageSize);

            httpContextWriter.AddPagination(filters.PageNumber, filters.PageSize,
                mtaLogs.TotalCount + (phoneSms != null ? phoneSms.TotalCount : 0),
                mtaLogs.TotalPages + (phoneSms != null ? phoneSms.TotalPages : 0));

            var mtaLogModels = mapper.Map<IEnumerable<MtaLogModel>>(mtaLogs);

            if (phoneSms != null)
                mtaLogModels = mtaLogModels.Concat(mapper.Map<IEnumerable<MtaLogModel>>(phoneSms));

            mtaLogModels = filters.SortType == DateSortType.Descending
                ? mtaLogModels.OrderByDescending(m => m.Date)
                : mtaLogModels.OrderBy(m => m.Date);

            return mtaLogModels;
        }

        public async Task<PagedList<ApiLogModel>> GetApiLogsFromFile(ApiLogFiltersParams filters)
        {
            var logsFilePath = BuildLogFilesPath(filters.Date);

            if (!filesManager.FileExists(logsFilePath))
                return new List<ApiLogModel>().ToPagedList<ApiLogModel>(filters.PageNumber, filters.PageSize);

            string[] logsJson = await filesManager.ReadFileLines(logsFilePath);
            ReplaceKeyWordsInJson(ref logsJson);

            var logs = ConvertLogsFileIntoList(logsJson);

            logs = FilterLogs(filters, logs);

            return logs.ToPagedList<ApiLogModel>(filters.PageNumber, filters.PageSize);
        }

        #region private

        private string BuildLogFilesPath(DateTime date) =>
            $"{filesManager.WebRootPath}/logs/log-{date.Year}{(date.Month < 10 ? $"0{date.Month}" : date.Month)}{(date.Day < 10 ? $"0{date.Day}" : date.Day)}.txt";

        private static IEnumerable<ApiLogModel> ConvertLogsFileIntoList(string[] logsJson)
        {
            foreach (var logJson in logsJson)
                yield return logJson.FromJSON<ApiLogModel>();
        }

        private static IEnumerable<ApiLogModel> FilterLogs(ApiLogFiltersParams filterses, IEnumerable<ApiLogModel> logs)
        {
            if (!string.IsNullOrEmpty(filterses.Message))
                logs = logs.Where(l => l.Message != null && l.Message.ToLower().Contains(filterses.Message.ToLower()));

            if (!string.IsNullOrEmpty(filterses.Level))
                logs = logs.Where(l => l.Level != null && l.Level.ToUpper().Contains(filterses.Level.ToUpper()));

            if (!string.IsNullOrEmpty(filterses.RequestMethod))
                logs = logs.Where(l =>
                    l.RequestMethod != null && l.RequestMethod.ToLower().Contains(filterses.RequestMethod.ToLower()));

            if (!string.IsNullOrEmpty(filterses.RequestPath))
                logs = logs.Where(l =>
                    l.RequestPath != null && l.RequestPath.ToLower().Contains(filterses.RequestPath.ToLower()));

            if (filterses.StatusCode != null)
                logs = logs.Where(l => l.StatusCode == filterses.StatusCode);

            if (filterses.MinTime != null)
                logs = logs.Where(l => l.Date >= filterses.MinTime);

            if (filterses.MaxTime != null)
                logs = logs.Where(l => l.Date <= filterses.MaxTime);

            logs = ApiLogSortTypeSmartEnum.FromValue((int) filterses.SortType).Sort(logs);

            return logs;
        }

        private void ReplaceKeyWordsInJson(ref string[] logsJson)
        {
            for (int i = 0; i < logsJson.Length; i++)
            {
                logsJson[i] = logsJson[i].Replace(LogKeyWordsDictionary.DateKey,
                    logKeyWordsDictionary[LogKeyWordsDictionary.DateKey]);
                logsJson[i] = logsJson[i].Replace(LogKeyWordsDictionary.MessageKey,
                    logKeyWordsDictionary[LogKeyWordsDictionary.MessageKey]);
                logsJson[i] = logsJson[i].Replace(LogKeyWordsDictionary.LevelKey,
                    logKeyWordsDictionary[LogKeyWordsDictionary.LevelKey]);
                logsJson[i] = logsJson[i].Replace(LogKeyWordsDictionary.ExceptionKey,
                    logKeyWordsDictionary[LogKeyWordsDictionary.ExceptionKey]);
            }
        }

        private static List<string> FetchSourceAffectedValues(MtaLogFiltersParams filters,
            IEnumerable<SourceAffectedModel> sourceAffectedModels)
        {
            var sourceAffectedValues = filters.SourceAffectedLogType switch
            {
                SourceAffectedLogType.Account or SourceAffectedLogType.Character => sourceAffectedModels
                    .Select(s => SourceAffectedLogDictionary.Map(s.Type, s.Id.ToString())).ToList(),
                SourceAffectedLogType.System or SourceAffectedLogType.Pefuel or SourceAffectedLogType.Petoll =>
                    new List<string> {SourceAffectedLogDictionary.Map(filters.SourceAffectedLogType)},
                _ => new List<string>
                    {SourceAffectedLogDictionary.Map(filters.SourceAffectedLogType, filters.SourceAffected)}
            };

            return sourceAffectedValues;
        }

        #endregion
    }
}