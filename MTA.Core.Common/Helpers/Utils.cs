using System;

namespace MTA.Core.Common.Helpers
{
    public static class Utils
    {
        #region guid

        public static string Id() => Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();

        public static string Token(int length = 8) => Guid.NewGuid().ToString("N").Substring(0, length).ToUpper();

        public static string NewGuid(int length = 16) =>
            Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);

        #endregion

        #region enum

        public static string EnumToString<T>(T value) where T : struct, IConvertible
            => Enum.GetName(typeof(T), value);

        #endregion
    }
}