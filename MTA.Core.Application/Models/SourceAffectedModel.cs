using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Models
{
    public record SourceAffectedModel
    (
        int? Id,
        string Value,
        SourceAffectedLogType Type
    );
}