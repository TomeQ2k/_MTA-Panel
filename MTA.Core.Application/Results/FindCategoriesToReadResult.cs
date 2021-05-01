using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Results
{
    public record FindCategoriesToReadResult
    (
        ReportCategoryType[] ReportCategoryTypes,
        bool IsOwner = false
    );
}