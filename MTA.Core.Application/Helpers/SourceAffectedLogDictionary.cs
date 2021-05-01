using System.Collections.Generic;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Helpers
{
    public static class SourceAffectedLogDictionary
    {
        private static Dictionary<SourceAffectedLogType, string> sourceAffectedLogDictionary =>
            new Dictionary<SourceAffectedLogType, string>()
            {
                {SourceAffectedLogType.System, "SYSTEM"},
                {SourceAffectedLogType.Account, "ac"},
                {SourceAffectedLogType.Character, "ch"},
                {SourceAffectedLogType.Vehicle, "ve"},
                {SourceAffectedLogType.Estate, "in"},
                {SourceAffectedLogType.Faction, "fa"},
                {SourceAffectedLogType.PhoneNumber, "ph"},
                {SourceAffectedLogType.Pefuel, "pefuel"},
                {SourceAffectedLogType.Petoll, "petoll"}
            };

        public static string Map(SourceAffectedLogType logType, string id = null)
            => id != null ? $"{sourceAffectedLogDictionary[logType]}{id}" : sourceAffectedLogDictionary[logType];
    }
}