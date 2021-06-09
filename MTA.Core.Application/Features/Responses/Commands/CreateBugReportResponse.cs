using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record CreateBugReportResponse : BaseResponse
    {
        public BugReportDto BugReport { get; init; }
        
        public CreateBugReportResponse(Error error = null) : base(error)
        {
        }
    }
}