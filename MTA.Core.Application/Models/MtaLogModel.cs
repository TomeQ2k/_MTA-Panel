using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Models
{
    public record MtaLogModel
    {
        public DateTime Date { get; init; }
        public LogAction Action { get; init; }
        public string Source { get; init; }
        public string Affected { get; init; }
        public string Content { get; init; }
    }
}