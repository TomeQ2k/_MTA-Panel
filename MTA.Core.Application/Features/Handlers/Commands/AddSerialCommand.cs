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
    public class AddSerialCommand : IRequestHandler<AddSerialRequest, AddSerialResponse>
    {
        private readonly IAccountManager accountManager;
        private readonly ICryptoService cryptoService;
        private readonly ISerialService serialService;

        public AddSerialCommand(IAccountManager accountManager, ICryptoService cryptoService,
            ISerialService serialService)
        {
            this.accountManager = accountManager;
            this.cryptoService = cryptoService;
            this.serialService = serialService;
        }

        public async Task<AddSerialResponse> Handle(AddSerialRequest request, CancellationToken cancellationToken)
        {
            var (token, email, serial) = (cryptoService.Decrypt(request.Token),
                cryptoService.Decrypt(request.Email), cryptoService.Decrypt(request.Serial));

            if (await serialService.SerialExists(serial))
                throw new DuplicateException("Serial already exists");

            return await accountManager.AddSerial(serial, email, token)
                ? (AddSerialResponse) new AddSerialResponse().LogInformation(
                    $"User with email {email} added serial to their account")
                : throw new ChangeEmailException("Error occured during changing email");
        }
    }
}