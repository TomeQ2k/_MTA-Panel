using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record AddSerialSlotResponse : BaseResponse
    {
        public int CurrentSerialsLimit { get; init; }

        public AddSerialSlotResponse(Error error = null) : base(error)
        {
        }
    }
}