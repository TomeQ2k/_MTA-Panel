using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record EditEmailTemplateResponse : BaseResponse
    {
        public EditEmailTemplateResponse(Error error = null) : base(error)
        {
        }
    }
}