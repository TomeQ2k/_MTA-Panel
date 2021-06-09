using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class SendActivationEmailCommand : IRequestHandler<SendActivationEmailRequest, SendActivationEmailResponse>
    {
        private readonly IAuthService authService;
        private readonly ICryptoService cryptoService;
        private readonly IEmailSender emailSender;
        private readonly IReadOnlyEmailTemplateGenerator emailTemplateGenerator;

        public IConfiguration Configuration { get; }

        public SendActivationEmailCommand(IAuthService authService, ICryptoService cryptoService,
            IEmailSender emailSender, IReadOnlyEmailTemplateGenerator emailTemplateGenerator,
            IConfiguration configuration)
        {
            this.authService = authService;
            this.cryptoService = cryptoService;
            this.emailSender = emailSender;
            this.emailTemplateGenerator = emailTemplateGenerator;
            Configuration = configuration;
        }

        public async Task<SendActivationEmailResponse> Handle(SendActivationEmailRequest request,
            CancellationToken cancellationToken)
        {
            var result = await authService.GenerateActivationEmailToken(request.Email);

            if (result == null)
                return new SendActivationEmailResponse();

            var (encryptedToken, encryptedEmail) =
                (cryptoService.Encrypt(result.Token), cryptoService.Encrypt(result.Email));

            string callbackUrl =
                $"{Configuration.GetValue<string>(AppSettingsKeys.ServerAddress)}api/auth/confirm?email={encryptedEmail}&token={encryptedToken}";

            var emailTemplate =
                (await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate))
                .ReplaceParameters(new EmailTemplateParameter("{{username}}", result.Username),
                    new EmailTemplateParameter("{{callbackUrl}}", callbackUrl));

            return await emailSender.Send(EmailMessages.ActivationAccountEmail(result.Email, emailTemplate))
                ? new SendActivationEmailResponse {Token = encryptedToken, Email = result.Email}
                : new SendActivationEmailResponse();
        }
    }
}