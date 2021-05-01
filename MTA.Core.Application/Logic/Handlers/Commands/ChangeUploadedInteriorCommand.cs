using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class ChangeUploadedInteriorCommand
        : IRequestHandler<ChangeUploadedInteriorRequest, ChangeUploadedInteriorResponse>
    {
        private readonly IPremiumUserLibraryManager premiumUserLibraryManager;

        public ChangeUploadedInteriorCommand(IPremiumUserLibraryManager premiumUserLibraryManager)
        {
            this.premiumUserLibraryManager = premiumUserLibraryManager;
        }

        public async Task<ChangeUploadedInteriorResponse> Handle(ChangeUploadedInteriorRequest request,
            CancellationToken cancellationToken)
            => await premiumUserLibraryManager.ChangeUploadedInteriorFile(request.InteriorFile, request.OldFileId)
                ? new ChangeUploadedInteriorResponse()
                : throw new PremiumOperationException("Changing interior file failed");
    }
}