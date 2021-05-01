using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportSubscriberService : IReportSubscriberService
    {
        private readonly IDatabase database;

        public ReportSubscriberService(IDatabase database)
        {
            this.database = database;
        }

        public async Task<ReportSubscriber> AddSubscriber(Report report, int userId)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            if (report.CreatorId == userId || report.AssigneeId == userId)
                throw new DuplicateException("This user is already member of that report");

            if (await database.ReportSubscriberRepository.Find(new SqlBuilder().Append("reportId").Equals
                .Append($"'{report.Id}'")
                .And.Append("userId").Equals.Append(userId).Build().Query) != null)
                throw new DuplicateException("This user is already member of that report");

            var reportSubscriber = ReportSubscriber.Create(report.Id, userId);

            return await database.ReportSubscriberRepository.Insert(reportSubscriber, false)
                ? reportSubscriber
                : throw new DatabaseException();
        }

        public async Task<bool> RemoveSubscriber(Report report, int userId)
        {
            if (report == null)
                throw new EntityNotFoundException("Report not found");

            if (report.CreatorId == userId || report.AssigneeId == userId)
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var reportSubscriber = await database.ReportSubscriberRepository.Find(new SqlBuilder().Append("reportId")
                                       .Equals
                                       .Append($"'{report.Id}'")
                                       .And.Append("userId").Equals.Append(userId).Build().Query) ??
                                   throw new EntityNotFoundException("Report subscriber not found");

            return await database.ReportSubscriberRepository.Delete(reportSubscriber);
        }
    }
}