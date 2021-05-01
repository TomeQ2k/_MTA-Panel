namespace MTA.Core.Application.Results
{
    public record AddSerialSlotResult
    (
        bool IsSucceeded,
        int CurrentSerialsLimit
    );
}