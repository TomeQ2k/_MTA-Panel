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
    public class SendAddSerialEmailCommand : IRequestHandler<SendAddSerialEmailRequest, SendAddSerialEmailResponse>
    {
        private readonly ISerialService serialService;
        private readonly IUserService userService;
        private readonly ICryptoService cryptoService;
        private readonly IEmailSender emailSender;
        private readonly IUserTokenGenerator userTokenGenerator;
        private readonly IEmailTemplateGenerator emailTemplateGenerator;
        private readonly IHttpContextReader httpContextReader;
        public IConfiguration Configuration { get; }

        public SendAddSerialEmailCommand(ISerialService serialService, IUserService userService,
            ICryptoService cryptoService, IEmailSender emailSender, IUserTokenGenerator userTokenGenerator,
            IEmailTemplateGenerator emailTemplateGenerator, IHttpContextReader httpContextReader,
            IConfiguration configuration)
        {
            this.serialService = serialService;
            this.userService = userService;
            this.cryptoService = cryptoService;
            this.emailSender = emailSender;
            this.userTokenGenerator = userTokenGenerator;
            this.emailTemplateGenerator = emailTemplateGenerator;
            this.httpContextReader = httpContextReader;

            Configuration = configuration;
        }

        public async Task<SendAddSerialEmailResponse> Handle(SendAddSerialEmailRequest request,
            CancellationToken cancellationToken)
        {
            var user = await userService.GetUserWithSerials(httpContextReader.CurrentUserId);
            
            if (!(await userService.GetUserWithSerials(httpContextReader.CurrentUserId)).HasEmptySerialSlot())
                throw new PremiumOperationException("Account has no more serial slots");

            if (await serialService.SerialExists(request.Serial, httpContextReader.CurrentUserId))
                throw new DuplicateException("Serial already exists");

            var result = await userTokenGenerator.GenerateAddSerialToken();

            var (encryptedSerial, encryptedEmail, encryptedToken) = (cryptoService.Encrypt(request.Serial),
                cryptoService.Encrypt(result.Email),
                cryptoService.Encrypt(result.Token));

            string callbackUrl =
                $"{Configuration.GetValue<string>(AppSettingsKeys.ServerAddress)}api/account/serial/add?email={encryptedEmail}&token={encryptedToken}&serial={encryptedSerial}";

            var emailTemplate =
                (await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate))
                .ReplaceParameters(new EmailTemplateParameter("{{username}}", result.Username),
                    new EmailTemplateParameter("{{callbackUrl}}", callbackUrl));

            await emailSender.Send(EmailMessages.ActivationAccountEmail(result.Email, emailTemplate));

            return new SendAddSerialEmailResponse();
        }
    }
}