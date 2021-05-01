using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportCommentService : IReportCommentService
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public ReportCommentService(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<ReportComment> AddComment(AddReportCommentRequest request)
        {
            var reportComment = new ReportCommentBuilder()
                .AddedTo(request.ReportId)
                .CreatedBy(httpContextReader.CurrentUserId)
                .SetContent(request.Content)
                .SetPrivacy(request.IsPrivate)
                .Build();

            return await database.ReportCommentRepository.Insert(reportComment, false)
                ? reportComment
                : throw new DatabaseException();
        }
    }
}