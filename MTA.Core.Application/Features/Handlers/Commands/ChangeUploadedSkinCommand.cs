using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class ChangeUploadedSkinCommand : IRequestHandler<ChangeUploadedSkinRequest, ChangeUploadedSkinResponse>
    {
        private readonly IPremiumUserLibraryManager premiumUserLibraryManager;

        public ChangeUploadedSkinCommand(IPremiumUserLibraryManager premiumUserLibraryManager)
        {
            this.premiumUserLibraryManager = premiumUserLibraryManager;
        }

        public async Task<ChangeUploadedSkinResponse> Handle(ChangeUploadedSkinRequest request,
            CancellationToken cancellationToken)
            => await premiumUserLibraryManager.ChangeUploadedSkinFile(request.SkinFile, request.OldFileId,
                request.SkinId)
                ? new ChangeUploadedSkinResponse()
                : throw new PremiumOperationException("Changing skin file failed");
    }
}