using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ArchiveReportResponse : BaseResponse
    {
        public ArchiveReportResponse(Error error = null) : base(error)
        {
        }
    }
}