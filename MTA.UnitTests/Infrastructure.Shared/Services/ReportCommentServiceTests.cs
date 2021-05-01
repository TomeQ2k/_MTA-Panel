using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportCommentServiceTests
    {
        private ReportCommentService reportCommentService;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;

        private AddReportCommentRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new AddReportCommentRequest();

            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.ReportCommentRepository.Insert(It.IsNotNull<ReportComment>(), false))
                .ReturnsAsync(true);

            reportCommentService = new ReportCommentService(database.Object, httpContextReader.Object);
        }

        [Test]
        public void AddComment_InsertingReportCommentFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportCommentRepository.Insert(It.IsNotNull<ReportComment>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportCommentService.AddComment(request), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task AddComment_WhenCalled_ReturnReportComment()
        {
            var result = await reportCommentService.AddComment(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ReportComment>());
        }
    }
}