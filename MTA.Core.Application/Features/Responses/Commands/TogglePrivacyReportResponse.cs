using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record TogglePrivacyReportResponse : BaseResponse
    {
        public bool IsPrivate { get; init; }

        public TogglePrivacyReportResponse(Error error = null) : base(error)
        {
        }
    }
}