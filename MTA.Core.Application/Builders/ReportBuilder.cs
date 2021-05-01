using System;
using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly Report report = new();

        public IReportBuilder SetSubject(string subject)
        {
            report.SetSubject(subject);
            return this;
        }

        public IReportBuilder SetContent(string content)
        {
            report.SetContent(content);
            return this;
        }

        public IReportBuilder SetCategoryType(ReportCategoryType categoryType)
        {
            report.SetCategoryType(categoryType);
            return this;
        }

        public IReportBuilder CreatedBy(int userId)
        {
            report.CreatedBy(userId);
            return this;
        }

        public IReportBuilder SetEventDate(DateTime? eventDate)
        {
            report.SetEventDate(eventDate);
            return this;
        }

        public IReportBuilder SetPrivacy(bool isPrivate)
        {
            report.SetPrivacy(isPrivate);
            return this;
        }

        public Report Build() => report;
    }
}