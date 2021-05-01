using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record CreateUserReportResponse : BaseResponse
    {
        public UserReportDto UserReport { get; init; }

        public CreateUserReportResponse(Error error = null) : base(error)
        {
        }
    }
}