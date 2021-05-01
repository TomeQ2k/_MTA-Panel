using System.Text.Json;

namespace MTA.Core.Application.Settings
{
    public static class JsonSettings
    {
        public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}