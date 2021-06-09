using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record CleanAccountResponse : BaseResponse
    {
        public CleanAccountResponse(Error error = null) : base(error)
        {
        }
    }
}