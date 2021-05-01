using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class ResetPasswordCommand : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
    {
        private readonly IResetPasswordManager resetPasswordManager;
        private readonly ICryptoService cryptoService;

        public ResetPasswordCommand(IResetPasswordManager resetPasswordManager, ICryptoService cryptoService)
        {
            this.resetPasswordManager = resetPasswordManager;
            this.cryptoService = cryptoService;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var (token, email) = (cryptoService.Decrypt(request.Token), cryptoService.Decrypt(request.Email));

            return await resetPasswordManager.ResetPassword(email, token, request.NewPassword)
                ? (ResetPasswordResponse) new ResetPasswordResponse().LogInformation(
                    $"User with email {email} has resetted their password")
                : throw new ResetPasswordException("Error occured during resetting password");
        }
    }
}