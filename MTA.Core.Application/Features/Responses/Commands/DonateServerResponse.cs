using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record DonateServerResponse : BaseResponse
    {
        public int CreditsAdded { get; init; }

        public DonateServerResponse(Error error = null) : base(error)
        {
        }
    }
}