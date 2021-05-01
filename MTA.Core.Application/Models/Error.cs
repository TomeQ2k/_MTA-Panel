using System.Net;

namespace MTA.Core.Application.Models
{
    public record Error
    {
        public string ErrorCode { get; init; }
        public string Message { get; init; }
        public HttpStatusCode StatusCode { get; init; }

        public static Error Build(string errorCode, string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) => new Error
        {
            ErrorCode = errorCode,
            Message = message,
            StatusCode = statusCode
        };
    }
}