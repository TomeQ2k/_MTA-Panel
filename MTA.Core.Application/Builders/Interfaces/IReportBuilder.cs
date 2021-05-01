using System;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IReportBuilder : IBuilder<Report>
    {
        IReportBuilder SetSubject(string subject);
        IReportBuilder SetContent(string content);
        IReportBuilder SetCategoryType(ReportCategoryType categoryType);
        IReportBuilder CreatedBy(int userId);
        IReportBuilder SetEventDate(DateTime? eventDate);
        IReportBuilder SetPrivacy(bool isPrivate);
    }
}