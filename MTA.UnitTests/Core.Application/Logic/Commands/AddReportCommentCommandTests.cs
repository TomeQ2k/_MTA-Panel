using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AddReportCommentCommandTests
    {
        private AddReportCommentCommand addReportCommentCommand;

        private Mock<IReportCommentService> reportCommentService;
        private Mock<IReportValidationHub> reportValidationHub;
        private Mock<IReportManager> reportManager;
        private Mock<IMapper> mapper;

        private AddReportCommentRequest request;
        private ReportComment reportComment;
        private Report report;

        [SetUp]
        public void SetUp()
        {
            request = new AddReportCommentRequest {ReportId = "id"};

            reportComment = new ReportComment();
            report = new Report();

            reportCommentService = new Mock<IReportCommentService>();
            reportValidationHub = new Mock<IReportValidationHub>();
            reportManager = new Mock<IReportManager>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            mapper = new Mock<IMapper>();

            reportCommentService.Setup(rcs => rcs.AddComment(request)).ReturnsAsync(reportComment);
            reportValidationHub.Setup(rvh =>
                    rvh.ValidateAndReturnReport(It.IsNotNull<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);
            reportManager.Setup(rm => rm.ChangeStatus(It.IsAny<ReportStatusType>(), report)).ReturnsAsync(true);

            addReportCommentCommand =
                new AddReportCommentCommand(reportCommentService.Object, reportValidationHub.Object,
                    reportManager.Object, notifier.Object,
                    hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_ReportValidationFailed_ThrowNoPermissionsException()
        {
            reportValidationHub.Setup(
                    rvh => rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => addReportCommentCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_AddingReportCommentFailed_ThrowCrudException()
        {
            reportCommentService.Setup(rcs => rcs.AddComment(request)).ReturnsAsync(() => null);

            Assert.That(() => addReportCommentCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnReportComment()
        {
            var result = await addReportCommentCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result, Is.TypeOf<AddReportCommentResponse>());
        }
    }
}