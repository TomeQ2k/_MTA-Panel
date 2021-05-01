using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Results
{
    public record ObjectProtectionResult
    (
        bool IsSucceeded,
        ObjectProtectionType ProtectionType,
        int ObjectId,
        DateTime? ProtectedUntil = null
    );
}