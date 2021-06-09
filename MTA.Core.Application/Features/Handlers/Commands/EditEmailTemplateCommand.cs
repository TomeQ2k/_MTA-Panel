using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class EditEmailTemplateCommand : IRequestHandler<EditEmailTemplateRequest, EditEmailTemplateResponse>
    {
        private readonly IEmailTemplateGenerator emailTemplateGenerator;

        public EditEmailTemplateCommand(IEmailTemplateGenerator emailTemplateGenerator)
        {
            this.emailTemplateGenerator = emailTemplateGenerator;
        }

        public async Task<EditEmailTemplateResponse> Handle(EditEmailTemplateRequest request,
            CancellationToken cancellationToken)
        {
            await Task.Run(() =>
                emailTemplateGenerator.EditEmailTemplate(request.TemplateName, request.Subject, request.Content));

            return new EditEmailTemplateResponse();
        }
    }
}