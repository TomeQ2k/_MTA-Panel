using System;

namespace MTA.Core.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFullDate(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}