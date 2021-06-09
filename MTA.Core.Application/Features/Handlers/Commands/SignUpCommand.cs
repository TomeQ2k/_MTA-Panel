using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class SignUpCommand : IRequestHandler<SignUpRequest, SignUpResponse>
    {
        private readonly IAuthService authService;
        private readonly ISerialService serialService;
        private readonly IReadOnlyUserService userService;
        private readonly IEmailSender emailSender;
        private readonly ICryptoService cryptoService;
        private readonly IReadOnlyEmailTemplateGenerator emailTemplateGenerator;
        private readonly IAuthValidationService authValidationService;
        private readonly ICaptchaService captchaService;
        private readonly IMapper mapper;

        public IConfiguration Configuration { get; }

        public SignUpCommand(IAuthService authService, ISerialService serialService,
            IReadOnlyUserService userService, IEmailSender emailSender, ICryptoService cryptoService,
            IReadOnlyEmailTemplateGenerator emailTemplateGenerator, IAuthValidationService authValidationService,
            ICaptchaService captchaService, IConfiguration configuration, IMapper mapper)
        {
            this.authService = authService;
            this.serialService = serialService;
            this.userService = userService;
            this.emailSender = emailSender;
            this.cryptoService = cryptoService;
            this.emailTemplateGenerator = emailTemplateGenerator;
            this.authValidationService = authValidationService;
            this.captchaService = captchaService;
            this.mapper = mapper;

            Configuration = configuration;
        }

        public async Task<SignUpResponse> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            if (!await captchaService.VerifyCaptcha(request.CaptchaResponse))
                throw new CaptchaException();

            if (await authValidationService.UsernameExists(request.Username) ||
                await authValidationService.EmailExists(request.Email))
                throw new DuplicateException("Account already exists");

            if (await serialService.SerialExists(request.Serial))
                throw new DuplicateException("Serial already exists");

            User referrer = default;
            if (!string.IsNullOrEmpty(request.Referrer))
                referrer = await userService.FindUserByUsername(request.Referrer);

            var response = await authService.SignUp(request.Username, request.Email, request.Password, request.Serial,
                referrer == null ? 0 : referrer.Id);

            var (encryptedToken, encryptedEmail) = (cryptoService.Encrypt(response.TokenCode),
                cryptoService.Encrypt(response.User.Email));

            //TODO:Change it on ClientAddress
            string callbackUrl =
                $"{Configuration.GetValue<string>(AppSettingsKeys.ServerAddress)}api/auth/confirm?email={encryptedEmail}&token={encryptedToken}";

            var emailTemplate =
                (await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate))
                .ReplaceParameters(new EmailTemplateParameter("{{username}}", response.User.Username),
                    new EmailTemplateParameter("{{callbackUrl}}", callbackUrl));

            return await emailSender.Send(EmailMessages.ActivationAccountEmail(response.User.Email, emailTemplate))
                ? (SignUpResponse) new SignUpResponse
                        {TokenCode = encryptedToken, User = mapper.Map<UserAuthDto>(response.User)}
                    .LogInformation($"User {request.Username} with email {request.Email} signed up")
                : throw new ServiceException("Sending confirmation email failed");
        }
    }
}