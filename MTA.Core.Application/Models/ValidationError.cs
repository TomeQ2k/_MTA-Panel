namespace MTA.Core.Application.Models
{
    public record ValidationError
    {
        public string Field { get; init; }
        public string Message { get; init; }

        public ValidationError(string field, string message)
            => (Field, Message) = (field != string.Empty ? field : null, message);
    }
}