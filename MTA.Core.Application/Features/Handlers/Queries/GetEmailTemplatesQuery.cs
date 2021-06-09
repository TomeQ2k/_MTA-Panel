using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetEmailTemplatesQuery : IRequestHandler<GetEmailTemplatesRequest, GetEmailTemplatesResponse>
    {
        private readonly IReadOnlyEmailTemplateGenerator emailTemplateGenerator;

        public GetEmailTemplatesQuery(IReadOnlyEmailTemplateGenerator emailTemplateGenerator)
        {
            this.emailTemplateGenerator = emailTemplateGenerator;
        }

        public async Task<GetEmailTemplatesResponse> Handle(GetEmailTemplatesRequest request,
            CancellationToken cancellationToken)
            => new GetEmailTemplatesResponse {EmailTemplates = await emailTemplateGenerator.GetEmailTemplates()};
    }
}