using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class ReportImageBuilder : IReportImageBuilder
    {
        private readonly ReportImage reportImage = new();

        public IReportImageBuilder SetLocation(string path)
        {
            reportImage.SetLocation(path);
            return this;
        }

        public IReportImageBuilder SetReportId(string reportId)
        {
            reportImage.SetReportId(reportId);
            return this;
        }

        public IReportImageBuilder SetUserId(int userId)
        {
            reportImage.SetUserId(userId);
            return this;
        }

        public IReportImageBuilder SetFileSize(long size)
        {
            reportImage.SetFileSize(size);
            return this;
        }

        public ReportImage Build() => reportImage;
    }
}