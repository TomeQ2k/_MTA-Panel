using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetEmailTemplateResponse : BaseResponse
    {
        public EmailTemplate EmailTemplate { get; init; }

        public GetEmailTemplateResponse(Error error = null) : base(error)
        {
        }
    }
}