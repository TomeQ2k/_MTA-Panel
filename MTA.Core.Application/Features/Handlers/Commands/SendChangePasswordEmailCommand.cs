using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class
        SendChangePasswordEmailCommand : IRequestHandler<SendChangePasswordEmailRequest, SendChangePasswordEmailResponse
        >
    {
        private readonly IUserTokenGenerator userTokenGenerator;
        private readonly ICryptoService cryptoService;
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplateGenerator emailTemplateGenerator;
        public IConfiguration Configuration { get; }

        public SendChangePasswordEmailCommand(IUserTokenGenerator userTokenGenerator, ICryptoService cryptoService,
            IEmailSender emailSender, IEmailTemplateGenerator emailTemplateGenerator, IConfiguration configuration)
        {
            this.userTokenGenerator = userTokenGenerator;
            this.cryptoService = cryptoService;
            this.emailSender = emailSender;
            this.emailTemplateGenerator = emailTemplateGenerator;
            Configuration = configuration;
        }

        public async Task<SendChangePasswordEmailResponse> Handle(SendChangePasswordEmailRequest request,
            CancellationToken cancellationToken)
        {
            var result = await userTokenGenerator.GenerateChangePasswordToken(request.OldPassword);

            var (encryptedToken, encryptedEmail, encryptedPassword) =
                (cryptoService.Encrypt(result.Token), cryptoService.Encrypt(result.Email),
                    cryptoService.Encrypt(request.NewPassword));

            string callbackUrl =
                $"{Configuration.GetValue<string>(AppSettingsKeys.ServerAddress)}api/account/changePassword?email={encryptedEmail}&token={encryptedToken}&newPassword={encryptedPassword}";

            var emailTemplate =
                (await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate))
                .ReplaceParameters(new EmailTemplateParameter("{{username}}", result.Username),
                    new EmailTemplateParameter("{{callbackUrl}}", callbackUrl));

            await emailSender.Send(EmailMessages.ActivationAccountEmail(result.Email, emailTemplate));

            return new SendChangePasswordEmailResponse();
        }
    }
}