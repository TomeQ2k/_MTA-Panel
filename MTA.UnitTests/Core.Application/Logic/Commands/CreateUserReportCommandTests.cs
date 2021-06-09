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
    public class CreateUserReportCommandTests
    {
        private UserReport report;
        private CreateUserReportRequest request;
        private Mock<IReportService> reportService;
        private CreateUserReportCommnad createUserReportCommnad;
        private Mock<IReportManager> reportManager;

        [SetUp]
        public void SetUp()
        {
            report = new UserReport();
            request = new CreateUserReportRequest();

            var mapper = new Mock<IMapper>();
            reportService = new Mock<IReportService>();
            reportManager = new Mock<IReportManager>();

            reportService.Setup(rs => rs.CreateUserReport(It.IsAny<CreateUserReportRequest>()))
                .ReturnsAsync(report);
            mapper.Setup(m => m.Map<UserReportDto>(It.IsAny<UserReport>())).Returns(new UserReportDto());

            createUserReportCommnad =
                new CreateUserReportCommnad(reportService.Object, reportManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CreatingReportFailed_ThrowServerException()
        {
            reportService.Setup(rs => rs.CreateUserReport(It.IsAny<CreateUserReportRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createUserReportCommnad.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCreateOtherReportResponse()
        {
            var result = await createUserReportCommnad.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<CreateUserReportResponse>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserReport, Is.Not.Null);
        }
    }
}