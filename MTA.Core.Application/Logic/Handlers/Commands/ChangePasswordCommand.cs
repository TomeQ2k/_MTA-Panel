using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class ChangePasswordCommand : IRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
    {
        private readonly IAccountManager accountManager;
        private readonly ICryptoService cryptoService;

        public ChangePasswordCommand(IAccountManager accountManager, ICryptoService cryptoService)
        {
            this.accountManager = accountManager;
            this.cryptoService = cryptoService;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request,
            CancellationToken cancellationToken)
        {
            var (token, email, newPassword) = (cryptoService.Decrypt(request.Token),
                cryptoService.Decrypt(request.Email), cryptoService.Decrypt(request.NewPassword));

            return await accountManager.ChangePassword(newPassword, email, token)
                ? new ChangePasswordResponse()
                : throw new ChangePasswordException("Error occured during changing password");
        }
    }
}