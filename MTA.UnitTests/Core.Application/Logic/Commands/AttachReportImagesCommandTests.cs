using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AttachReportImagesCommandTests
    {
        private AttachReportImagesCommand attachReportImagesCommand;

        private Mock<IReportImageService> reportImageService;
        private Mock<IReportValidationHub> reportValidationHub;
        private Mock<IHttpContextReader> httpContextReader;

        private Report report;
        private AttachReportImagesRequest request;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new AttachReportImagesRequest();

            reportImageService = new Mock<IReportImageService>();
            reportValidationHub = new Mock<IReportValidationHub>();
            httpContextReader = new Mock<IHttpContextReader>();

            reportImageService.Setup(ris =>
                    ris.UploadReportImages(It.IsAny<int>(), It.IsAny<Report>(),
                        It.IsAny<ICollection<IFormFile>>()))
                .ReturnsAsync(true);
            reportValidationHub.Setup(
                    rvh => rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);
            httpContextReader.Setup(htr => htr.CurrentUserId).Returns(UserId);

            attachReportImagesCommand =
                new AttachReportImagesCommand(reportImageService.Object, reportValidationHub.Object,
                    httpContextReader.Object);
        }

        [Test]
        public void Handle_ReportValidationFailed_ThrowNoPermissionsException()
        {
            reportValidationHub.Setup(
                    rvh => rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => attachReportImagesCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_UploadingReportImagesFailed_ThrowUploadFileException()
        {
            reportImageService.Setup(ris =>
                    ris.UploadReportImages(It.IsAny<int>(), It.IsAny<Report>(),
                        It.IsAny<ICollection<IFormFile>>()))
                .ReturnsAsync(false);

            Assert.That(() => attachReportImagesCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<UploadFileException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAttachReportImagesResponse()
        {
            var result = await attachReportImagesCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result, Is.TypeOf<AttachReportImagesResponse>());
        }
    }
}