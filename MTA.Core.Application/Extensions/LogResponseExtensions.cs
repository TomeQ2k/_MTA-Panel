using MTA.Core.Application.Logic.Responses;

namespace MTA.Core.Application.Extensions
{
    public static class LogResponseExtensions
    {
        public static BaseResponse LogInformation(this BaseResponse response, string message)
        {
            Serilog.Log.Information(message);
            return response;
        }

        public static BaseResponse LogError(this BaseResponse response, string message)
        {
            Serilog.Log.Error(message);
            return response;
        }

        public static BaseResponse LogWarning(this BaseResponse response, string message)
        {
            Serilog.Log.Warning(message);
            return response;
        }
    }
}