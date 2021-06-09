using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class ConfirmAccountCommand : IRequestHandler<ConfirmAccountRequest, ConfirmAccountResponse>
    {
        private readonly IAuthService authService;
        private readonly ICryptoService cryptoService;

        public ConfirmAccountCommand(IAuthService authService, ICryptoService cryptoService)
        {
            this.authService = authService;
            this.cryptoService = cryptoService;
        }

        public async Task<ConfirmAccountResponse> Handle(ConfirmAccountRequest request,
            CancellationToken cancellationToken)
        {
            var (token, email) = (cryptoService.Decrypt(request.Token), cryptoService.Decrypt(request.Email));

            return await authService.ConfirmAccount(email, token)
                ? (ConfirmAccountResponse) new ConfirmAccountResponse().LogInformation(
                    $"User with email {email} confirmed their account")
                : throw new AccountNotConfirmedException("Error occured during account confirmation");
        }
    }
}