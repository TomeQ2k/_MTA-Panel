using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetEmailTemplateQuery : IRequestHandler<GetEmailTemplateRequest, GetEmailTemplateResponse>
    {
        private readonly IReadOnlyEmailTemplateGenerator emailTemplateGenerator;

        public GetEmailTemplateQuery(IReadOnlyEmailTemplateGenerator emailTemplateGenerator)
        {
            this.emailTemplateGenerator = emailTemplateGenerator;
        }

        public async Task<GetEmailTemplateResponse> Handle(GetEmailTemplateRequest request,
            CancellationToken cancellationToken)
            => new GetEmailTemplateResponse
                {EmailTemplate = await emailTemplateGenerator.FindEmailTemplate(request.TemplateName)};
    }
}