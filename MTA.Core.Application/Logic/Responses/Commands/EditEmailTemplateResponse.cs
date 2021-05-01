using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record EditEmailTemplateResponse : BaseResponse
    {
        public EditEmailTemplateResponse(Error error = null) : base(error)
        {
        }
    }
}