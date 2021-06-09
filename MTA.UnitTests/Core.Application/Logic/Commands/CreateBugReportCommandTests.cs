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
    public class CreateBugReportCommandTests
    {
        private BugReport report;
        private CreateBugReportRequest request;
        private Mock<IReportService> reportService;
        private CreateBugReportCommand createBugReportCommand;
        private Mock<IReportManager> reportManager;

        [SetUp]
        public void SetUp()
        {
            report = new BugReport();
            request = new CreateBugReportRequest();

            var mapper = new Mock<IMapper>();
            reportService = new Mock<IReportService>();
            reportManager = new Mock<IReportManager>();

            reportService.Setup(rs => rs.CreateBugReport(It.IsAny<CreateBugReportRequest>()))
                .ReturnsAsync(report);
            mapper.Setup(m => m.Map<BugReportDto>(It.IsAny<BugReport>())).Returns(new BugReportDto());

            createBugReportCommand =
                new CreateBugReportCommand(reportService.Object, reportManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CreatingReportFailed_ThrowServerException()
        {
            reportService.Setup(rs => rs.CreateBugReport(It.IsAny<CreateBugReportRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createBugReportCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCreateOtherReportResponse()
        {
            var result = await createBugReportCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<CreateBugReportResponse>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BugReport, Is.Not.Null);
        }
    }
}