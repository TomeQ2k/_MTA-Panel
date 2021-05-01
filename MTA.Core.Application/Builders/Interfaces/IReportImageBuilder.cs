using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IReportImageBuilder : IBuilder<ReportImage>
    {
        IReportImageBuilder SetLocation(string url, string path);
        IReportImageBuilder SetReportId(string reportId);
        IReportImageBuilder SetUserId(int userId);
        IReportImageBuilder SetFileSize(long size);
    }
}