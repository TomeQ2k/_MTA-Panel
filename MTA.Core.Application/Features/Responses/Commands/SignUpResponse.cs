using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SignUpResponse : BaseResponse
    {
        public string TokenCode { get; init; }
        public UserAuthDto User { get; init; }

        public SignUpResponse(Error error = null) : base(error)
        {
        }
    }
}