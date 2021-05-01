using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class SendChangeEmailEmailCommand : IRequestHandler<SendChangeEmailEmailRequest,
        SendChangeEmailEmailResponse>
    {
        private readonly IUserTokenGenerator userTokenGenerator;
        private readonly IAuthValidationService authValidationService;
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplateGenerator emailTemplateGenerator;
        private readonly ICryptoService cryptoService;

        public IConfiguration Configuration { get; }

        public SendChangeEmailEmailCommand(IUserTokenGenerator userTokenGenerator,
            IAuthValidationService authValidationService, IEmailSender emailSender,
            IEmailTemplateGenerator emailTemplateGenerator, ICryptoService cryptoService,
            IConfiguration configuration)
        {
            this.userTokenGenerator = userTokenGenerator;
            this.authValidationService = authValidationService;
            this.emailSender = emailSender;
            this.emailTemplateGenerator = emailTemplateGenerator;
            this.cryptoService = cryptoService;

            Configuration = configuration;
        }

        public async Task<SendChangeEmailEmailResponse> Handle(SendChangeEmailEmailRequest request,
            CancellationToken cancellationToken)
        {
            if (await authValidationService.EmailExists(request.NewEmail))
                throw new DuplicateException("Account with this email already exists");

            var result = await userTokenGenerator.GenerateChangeEmailToken();

            var (encryptedToken, encryptedEmail, encryptedNewEmail) =
                (cryptoService.Encrypt(result.Token), cryptoService.Encrypt(result.Email),
                    cryptoService.Encrypt(request.NewEmail));

            string callbackUrl =
                $"{Configuration.GetValue<string>(AppSettingsKeys.ServerAddress)}api/account/changeEmail?email={encryptedEmail}&token={encryptedToken}&newEmail={encryptedNewEmail}";

            var emailTemplate =
                (await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate))
                .ReplaceParameters(new EmailTemplateParameter("{{username}}", result.Username),
                    new EmailTemplateParameter("{{callbackUrl}}", callbackUrl));

            await emailSender.Send(EmailMessages.ActivationAccountEmail(result.Email, emailTemplate));

            return new SendChangeEmailEmailResponse();
        }
    }
}