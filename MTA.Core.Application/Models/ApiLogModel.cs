using System;
using MTA.Core.Application.Helpers;

namespace MTA.Core.Application.Models
{
    public record ApiLogModel
    (
        DateTime Date,
        string Message,
        string Level = LogLevel.Information,
        string Exception = null,
        string RequestMethod = null,
        string RequestPath = null,
        int? StatusCode = null,
        float? Elapsed = null,
        string SourceContext = null,
        string RequestId = null,
        string ConnectionId = null
    );
}