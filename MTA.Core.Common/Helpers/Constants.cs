using MTA.Core.Common.Enums;

namespace MTA.Core.Common.Helpers
{
    public static class Constants
    {
        #region values

        public const int MinimumUsernameLength = 5;
        public const int MaximumUsernameLength = 30;
        public const int MinimumEmailLength = 5;
        public const int MaximumEmailLength = 50;
        public const int MinimumPasswordLength = 6;
        public const int MaximumPasswordLength = 64;
        public const int SerialLength = 32;

        public const int MaximumEmailSubjectLength = 500;
        public const int MaximumEmailContentLength = 10000;

        public const int MinimumArticleTitleLength = 5;
        public const int MaximumArticleTitleLength = 100;
        public const int MinimumArticleContentLength = 10;
        public const int MaximumArticleContentLength = 200;
        public const int MaximumArticlePhotoSizeInMb = 5;

        public const int MaximumReviewRPTestNote = 1000;

        public const int MaximumBanReasonLength = 2000;

        public const int UnitConversionMultiplier = 1024;

        public const int MaximumInactiveDays = 30;

        public const int TopStatsLimit = 10;

        public const int StatsHostedServiceTimeInMinutes = 15;
        public const int TokenHostedServiceTimeInMinutes = 1440;
        public const int ReportHostedServiceTimeInMinutes = 30;
        public const int ApiLogHostedServiceTimeInMinutes = 1440;
        public const int DatabaseRestoreHostedServiceTimeInMinutes = 5;

        public const int JwtTokenExpireTimeInDays = 7;
        public const int ConfirmAccountTokenExpireDays = 2;
        public const int ResetPasswordTokenExpireDays = 2;
        public const int ChangePasswordTokenExpireDays = 2;
        public const int ReportArchivizationTimeInDays = 7;
        public const int PaymentTokenExpireTimeInHours = 3;
        public const int ApiLogTrashTimeInDays = 7;

        public const int ArticlesCount = 6;

        public const double RPTestPassLimit = 0.5;

        public const int MaximumReportSubjectLength = 1000;
        public const int MaximumReportContentLength = 5000;
        public const int MaximumReportCommentContentLength = 3500;
        public const int MaximumBugReportAdditionalInfoLength = 500;
        public const int MaximumAdminNoteLength = 500;

        public const int MaximumPaymentDescriptionLength = 500;

        public const int MaximumAssignedReportsCount = 6;

        public const long MaximumReportImageSize = 5 * UnitConversionMultiplier * UnitConversionMultiplier;
        public const long MaximumReportImageSizePerRequest = 10 * UnitConversionMultiplier * UnitConversionMultiplier;
        public const long MaximumReportImagesSizePerUser = 50 * UnitConversionMultiplier * UnitConversionMultiplier;

        public const long MaximumPremiumFileSize = 3 * UnitConversionMultiplier * UnitConversionMultiplier;

        public const int MaximumReportImagesCountPerUser = 10;

        public const string LogFilesPath = "./wwwroot/logs/log-.txt";
        public const int LogsPageSize = 50;
        public const int LogsActionsMaxCount = 5;

        public const int MaximumLogFilterLength = 250;

        public const int SearchFromDatabaseLimit = 50;

        public const int MaximumTempObjectsCount = 250;

        public static string DonationReportSubject(string orderId) => $"DONATION - #{orderId}";

        public static string DonationReportContent(int userId, string username) =>
            $"User {username} #{userId} donated server for {(decimal) DonationType.ThreeHundredSeventyFivePLN} PLN and buy DLC Brain";

        public static ReportCategoryType[] OwnerReportCategories =
        {
            ReportCategoryType.Question, ReportCategoryType.Ban, ReportCategoryType.Penalty, ReportCategoryType.User,
            ReportCategoryType.Donation, ReportCategoryType.Account, ReportCategoryType.Bug
        };

        public static ReportCategoryType[] AdminReportCategories =
        {
            ReportCategoryType.Question, ReportCategoryType.Ban, ReportCategoryType.Penalty, ReportCategoryType.User,
            ReportCategoryType.Account, ReportCategoryType.Bug
        };

        public static ReportCategoryType[] SupporterReportCategories =
        {
            ReportCategoryType.Question, ReportCategoryType.Penalty, ReportCategoryType.User, ReportCategoryType.Bug
        };

        #endregion

        #region policies

        public const string OwnerPolicy = "OwnerPolicy";
        public const string AdminsPolicy = "AdminsPolicy";
        public const string AllAdminsPolicy = "AllAdminsPolicy";
        public const string AllOwnersPolicy = "AllOwnersPolicy";
        public const string AdminsAndSupportersPolicy = "AdminsAndSupportersPolicy";
        public const string TeamPolicy = "TeamPolicy";

        public const string CorsPolicy = "CorsPolicy";

        #endregion

        #region roles

        public const RoleType OwnerRole = RoleType.Owner;

        public static RoleType[] AdminRoles =
        {
            RoleType.Owner, RoleType.ViceOwner, RoleType.Admin
        };

        public static RoleType[] AllAdminsRoles =
        {
            RoleType.Owner, RoleType.ViceOwner, RoleType.Admin, RoleType.TrialAdmin
        };

        public static RoleType[] AllOwnersRoles =
        {
            RoleType.Owner, RoleType.ViceOwner
        };

        public static RoleType[] AdminsAndSupportersRoles =
        {
            RoleType.Owner, RoleType.ViceOwner, RoleType.Admin, RoleType.TrialAdmin, RoleType.SupporterLeader,
            RoleType.Supporter
        };

        public static RoleType[] TeamRoles =
        {
            RoleType.Owner, RoleType.ViceOwner, RoleType.Admin, RoleType.TrialAdmin, RoleType.SupporterLeader,
            RoleType.Supporter, RoleType.VctLeader, RoleType.Vct, RoleType.MapperLeader, RoleType.Mapper,
            RoleType.Scripter
        };

        public static RoleType[] VctRoles =
        {
            RoleType.Vct, RoleType.VctLeader
        };

        public static RoleType[] MappersRoles =
        {
            RoleType.Mapper, RoleType.MapperLeader
        };

        public static RoleType[] ScriptersRoles =
        {
            RoleType.Tester, RoleType.TrialScripter, RoleType.Scripter
        };

        #endregion

        #region regex

        public const string UsernameRegex = @"^[A-Za-z0-9-_]*$";
        public const string SerialRegex = @"^[A-Z0-9]*$";
        public const string IsEmailAddressRegex = @"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)";
        public const string PhoneNumberRegex = @"^[0-9]{0,6}$";

        #endregion
    }
}