using System.Collections.Generic;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetEmailTemplatesResponse : BaseResponse
    {
        public IEnumerable<EmailTemplate> EmailTemplates { get; init; }

        public GetEmailTemplatesResponse(Error error = null) : base(error)
        {
        }
    }
}