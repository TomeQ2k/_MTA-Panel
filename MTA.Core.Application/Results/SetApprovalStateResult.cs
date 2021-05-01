namespace MTA.Core.Application.Results
{
    public record SetApprovalStateResult
    (
        bool IsSucceeded,
        int UserId
    );
}