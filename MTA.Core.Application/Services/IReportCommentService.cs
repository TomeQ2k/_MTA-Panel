using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportCommentService
    {
        Task<ReportComment> AddComment(AddReportCommentRequest request);
    }
}