using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SendActivationEmailResponse : BaseResponse
    {
        public string Email { get; init; }
        public string Token { get; init; }

        public SendActivationEmailResponse(Error error = null) : base(error)
        {
        }
    }
}