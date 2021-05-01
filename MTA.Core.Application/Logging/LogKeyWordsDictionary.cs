using System.Collections.Generic;

namespace MTA.Core.Application.Logging
{
    public class LogKeyWordsDictionary : Dictionary<string, string>
    {
        public const string DateKey = "@t";
        public const string MessageKey = "@mt";
        public const string LevelKey = "@l";
        public const string ExceptionKey = "@x";

        public static LogKeyWordsDictionary Build() => new LogKeyWordsDictionary
        {
            {DateKey, "Date"},
            {MessageKey, "Message"},
            {LevelKey, "Level"},
            {ExceptionKey, "Exception"}
        };
    }
}