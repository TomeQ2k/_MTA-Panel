using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SignInResponse : BaseResponse
    {
        public string Token { get; init; }
        public UserAuthDto User { get; init; }
        
        public SignInResponse(Error error = null) : base(error)
        {
        }
    }
}