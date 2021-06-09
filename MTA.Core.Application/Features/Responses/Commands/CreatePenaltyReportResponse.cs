using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record CreatePenaltyReportResponse : BaseResponse
    {
        public PenaltyReportDto PenaltyReport { get; init; }

        public CreatePenaltyReportResponse(Error error = null) : base(error)
        {
        }
    }
}