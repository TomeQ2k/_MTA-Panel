using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class ChangeEmailCommand : IRequestHandler<ChangeEmailRequest, ChangeEmailResponse>
    {
        private readonly IAccountManager accountManager;
        private readonly ICryptoService cryptoService;
        private readonly IAuthValidationService authValidationService;

        public ChangeEmailCommand(IAccountManager accountManager, ICryptoService cryptoService,
            IAuthValidationService authValidationService)
        {
            this.accountManager = accountManager;
            this.cryptoService = cryptoService;
            this.authValidationService = authValidationService;
        }

        public async Task<ChangeEmailResponse> Handle(ChangeEmailRequest request, CancellationToken cancellationToken)
        {
            var (token, email, newEmail) = (cryptoService.Decrypt(request.Token),
                cryptoService.Decrypt(request.Email), cryptoService.Decrypt(request.NewEmail));

            if (await authValidationService.EmailExists(newEmail))
                throw new DuplicateException("Account with this email already exists");

            return await accountManager.ChangeEmail(newEmail, email, token)
                ? new ChangeEmailResponse()
                : throw new ChangeEmailException("Error occured during changing email");
        }
    }
}