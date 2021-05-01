using System;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.BackgroundServices
{
    internal class ReportHostedService : ServerHostedService
    {
        private static ReportCategoryType[] ReportCategoryTypes =
        {
            ReportCategoryType.Question, ReportCategoryType.Ban, ReportCategoryType.Penalty, ReportCategoryType.User,
            ReportCategoryType.Donation, ReportCategoryType.Account, ReportCategoryType.Bug
        };

        public ReportHostedService(IServiceProvider services) : base(services)
        {
            TimeInterval = Constants.ReportHostedServiceTimeInMinutes;
        }

        public override async void Callback(object state)
        {
            using (var scope = services.CreateScope())
            {
                var reportManager = scope.ServiceProvider.GetRequiredService<IReportManager>();

                foreach (var reportCategoryType in ReportCategoryTypes)
                {
                    await reportManager.AssignAwaitingReports(reportCategoryType, false);
                    await reportManager.AssignAwaitingReports(reportCategoryType, true);
                }

                var archiveReportsResult = await reportManager.ArchiveReports();

                Log.Information("Awaiting reports have been assigned");
                Log.Information($"Archived reports count: {archiveReportsResult.ArchivedReportsCount}");
                base.Callback(state);
            }
        }
    }
}