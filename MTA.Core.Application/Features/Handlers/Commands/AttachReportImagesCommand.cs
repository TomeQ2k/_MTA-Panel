using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class AttachReportImagesCommand : IRequestHandler<AttachReportImagesRequest, AttachReportImagesResponse>
    {
        private readonly IReportImageService reportImageService;
        private readonly IReportValidationHub reportValidationHub;
        private readonly IHttpContextReader httpContextReader;

        public AttachReportImagesCommand(IReportImageService reportImageService,
            IReportValidationHub reportValidationHub, IHttpContextReader httpContextReader)
        {
            this.reportImageService = reportImageService;
            this.reportValidationHub = reportValidationHub;
            this.httpContextReader = httpContextReader;
        }

        public async Task<AttachReportImagesResponse> Handle(AttachReportImagesRequest request,
            CancellationToken cancellationToken)
        {
            var report =
                await reportValidationHub.ValidateAndReturnReport(request.ReportId, ReportPermission.AddComment)
                ?? throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);
            ;

            return await reportImageService.UploadReportImages(httpContextReader.CurrentUserId, report,
                request.Images)
                ? new AttachReportImagesResponse()
                : throw new UploadFileException();
        }
    }
}