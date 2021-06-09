using System.Collections.Generic;

namespace MTA.Core.Application.Validation
{
    public record ValidationError
    {
        public string Field { get; }
        public IEnumerable<string> Messages { get; }

        public ValidationError(string field, IEnumerable<string> messages)
            => (Field, Messages) = (field != string.Empty ? field : null, messages);
    }
}