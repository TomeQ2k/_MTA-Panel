using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class
        SendResetPasswordByAdminCommand : IRequestHandler<SendResetPasswordByAdminRequest,
            SendResetPasswordByAdminResponse>
    {
        private readonly IUserTokenGenerator userTokenGenerator;
        private readonly ICryptoService cryptoService;
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplateGenerator emailTemplateGenerator;

        public IConfiguration Configuration { get; }

        public SendResetPasswordByAdminCommand(IUserTokenGenerator userTokenGenerator, ICryptoService cryptoService,
            IEmailSender emailSender, IEmailTemplateGenerator emailTemplateGenerator, IConfiguration configuration)
        {
            this.userTokenGenerator = userTokenGenerator;
            this.cryptoService = cryptoService;
            this.emailSender = emailSender;
            this.emailTemplateGenerator = emailTemplateGenerator;

            Configuration = configuration;
        }

        public async Task<SendResetPasswordByAdminResponse> Handle(SendResetPasswordByAdminRequest request,
            CancellationToken cancellationToken)
        {
            var result = await userTokenGenerator.GenerateResetPasswordToken(request.Login) ??
                         throw new CannotGenerateTokenException("Generating reset password token failed");

            var (encryptedToken, encryptedEmail) =
                (cryptoService.Encrypt(result.Token), cryptoService.Encrypt(result.Email));

            string callbackUrl =
                $"{Configuration.GetValue<string>(AppSettingsKeys.ServerAddress)}api/auth/resetPassword/verify?email={encryptedEmail}&token={encryptedToken}";

            var emailTemplate =
                (await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate))
                .ReplaceParameters(new EmailTemplateParameter("{{username}}", result.Username),
                    new EmailTemplateParameter("{{callbackUrl}}", callbackUrl));

            await emailSender.Send(EmailMessages.ActivationAccountEmail(result.Email, emailTemplate));

            return new SendResetPasswordByAdminResponse();
        }
    }
}