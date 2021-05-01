using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record CreatePenaltyReportResponse : BaseResponse
    {
        public PenaltyReportDto PenaltyReport { get; init; }

        public CreatePenaltyReportResponse(Error error = null) : base(error)
        {
        }
    }
}