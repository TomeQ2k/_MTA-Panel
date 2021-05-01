using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class ReportCommentBuilder : IReportCommentBuilder
    {
        private readonly ReportComment reportComment = new ReportComment();

        public IReportCommentBuilder AddedTo(string reportId)
        {
            reportComment.SetReportId(reportId);
            return this;
        }

        public IReportCommentBuilder CreatedBy(int userId)
        {
            reportComment.SetUserId(userId);
            return this;
        }

        public IReportCommentBuilder SetContent(string content)
        {
            reportComment.SetContent(content);
            return this;
        }

        public IReportCommentBuilder SetPrivacy(bool isPrivate)
        {
            reportComment.SetPrivacy(isPrivate);
            return this;
        }

        public ReportComment Build() => this.reportComment;
    }
}