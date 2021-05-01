using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
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