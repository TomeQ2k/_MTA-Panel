namespace MTA.Core.Application.Results
{
    public record ArchiveReportsResult
    (
        int ArchivedReportsCount,
        bool IsSucceeded
    );
}