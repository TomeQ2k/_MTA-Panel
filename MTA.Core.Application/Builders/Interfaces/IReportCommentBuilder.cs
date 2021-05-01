using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IReportCommentBuilder : IBuilder<ReportComment>
    {
        IReportCommentBuilder AddedTo(string reportId);
        IReportCommentBuilder CreatedBy(int userId);
        IReportCommentBuilder SetContent(string content);
        IReportCommentBuilder SetPrivacy(bool isPrivate);
    }
}