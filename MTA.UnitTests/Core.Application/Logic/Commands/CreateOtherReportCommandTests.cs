using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class CreateOtherReportCommandTests
    {
        private Report report;
        private CreateOtherReportRequest request;
        private Mock<IReportService> reportService;
        private Mock<IReportManager> reportManager;
        private CreateOtherReportCommand createOtherReportCommand;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new CreateOtherReportRequest();

            var mapper = new Mock<IMapper>();
            reportService = new Mock<IReportService>();
            reportManager = new Mock<IReportManager>();

            reportService.Setup(rs => rs.CreateOtherReport(It.IsAny<CreateOtherReportRequest>()))
                .ReturnsAsync(report);
            mapper.Setup(m => m.Map<ReportDto>(It.IsAny<Report>())).Returns(new ReportDto());

            createOtherReportCommand =
                new CreateOtherReportCommand(reportService.Object, reportManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CreatingReportFailed_ThrowServerException()
        {
            reportService.Setup(rs => rs.CreateOtherReport(It.IsAny<CreateOtherReportRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createOtherReportCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCreateOtherReportResponse()
        {
            var result = await createOtherReportCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<CreateOtherReportResponse>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Report, Is.Not.Null);
        }
    }
}