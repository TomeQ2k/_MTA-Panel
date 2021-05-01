namespace MTA.Core.Application.Results
{
    public record BlockCharacterResult
    (
        bool IsSucceeded,
        bool IsBlocked,
        string CharacterName,
        int AccountId
    );
}