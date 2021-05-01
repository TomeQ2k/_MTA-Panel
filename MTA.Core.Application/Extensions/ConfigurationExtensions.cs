using Microsoft.Extensions.Configuration;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsDev(this IConfiguration configuration, int userId)
            => userId == configuration.GetValue<int>(AppSettingsKeys.DevId);
    }
}