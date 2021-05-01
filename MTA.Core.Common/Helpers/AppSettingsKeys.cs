namespace MTA.Core.Common.Helpers
{
    public static class AppSettingsKeys
    {
        #region constants

        public const string ConnectionString = "DatabaseConnectionString";
        public const string ServerAddress = "Constants:ServerAddress";
        public const string ClientAddress = "Constants:ClientAddress";
        public const string Token = "Constants:Token";
        public const string TLSToken = "Constants:TLSToken";
        public const string DevId = "Constants:DevId";

        #endregion

        #region sections

        public const string IpRateLimitingSection = "IpRateLimiting";
        public const string CaptchaSection = "CaptchaSettings";

        #endregion
    }
}