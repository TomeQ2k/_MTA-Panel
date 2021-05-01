using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class VerifyResetPasswordQuery : IRequestHandler<VerifyResetPasswordRequest, VerifyResetPasswordResponse>
    {
        private readonly IResetPasswordManager resetPasswordManager;
        private readonly ICryptoService cryptoService;

        public VerifyResetPasswordQuery(IResetPasswordManager resetPasswordManager, ICryptoService cryptoService)
        {
            this.resetPasswordManager = resetPasswordManager;
            this.cryptoService = cryptoService;
        }

        public async Task<VerifyResetPasswordResponse> Handle(VerifyResetPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var (token, email) = (cryptoService.Decrypt(request.Token), cryptoService.Decrypt(request.Email));

            await resetPasswordManager.VerifyResetPasswordToken(email, token);

            return new VerifyResetPasswordResponse();
        }
    }
}