using System.Collections.Generic;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Helpers
{
    public static class ReportCategoryPermissionDictionary
    {
        public static Dictionary<RoleType, ReportCategoryType[]> ReportCategoryPermissions =>
            new Dictionary<RoleType, ReportCategoryType[]>
            {
                {RoleType.Owner, Constants.OwnerReportCategories},
                {RoleType.ViceOwner, Constants.OwnerReportCategories},
                {RoleType.Admin, Constants.AdminReportCategories},
                {RoleType.TrialAdmin, Constants.AdminReportCategories},
                {RoleType.SupporterLeader, Constants.SupporterReportCategories},
                {RoleType.Supporter, Constants.SupporterReportCategories},
            };
    }
}