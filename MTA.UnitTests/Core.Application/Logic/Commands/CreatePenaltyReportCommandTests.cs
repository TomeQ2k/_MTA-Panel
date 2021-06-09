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
    public class CreatePenaltyReportCommandTests
    {
        private PenaltyReport report;
        private CreatePenaltyReportRequest request;
        private Mock<IReportService> reportService;
        private CreatePenaltyReportCommand createPenaltyReportCommand;
        private Mock<IReportManager> reportManager;

        [SetUp]
        public void SetUp()
        {
            report = new PenaltyReport();
            request = new CreatePenaltyReportRequest();

            var mapper = new Mock<IMapper>();
            reportService = new Mock<IReportService>();
            reportManager = new Mock<IReportManager>();

            reportService.Setup(rs => rs.CreatePenaltyReport(It.IsAny<CreatePenaltyReportRequest>()))
                .ReturnsAsync(report);
            mapper.Setup(m => m.Map<PenaltyReportDto>(It.IsAny<PenaltyReport>())).Returns(new PenaltyReportDto());
                
            createPenaltyReportCommand = new CreatePenaltyReportCommand(reportService.Object, reportManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CreatingReportFailed_ThrowServerException()
        {
            reportService.Setup(rs => rs.CreatePenaltyReport(It.IsAny<CreatePenaltyReportRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createPenaltyReportCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCreateOtherReportResponse()
        {
            var result = await createPenaltyReportCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<CreatePenaltyReportResponse>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PenaltyReport, Is.Not.Null);

        }
    }
}