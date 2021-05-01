using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Logic.Handlers.Queries;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Queries
{
    [TestFixture]
    public class GetAuthValidationsQueryTests
    {
        private Mock<IAuthValidationService> authValidationService;
        private GetAuthValidationsRequest request;
        private GetAuthValidationsQuery getAuthValidationsQuery;

        [SetUp]
        public void SetUp()
        {
            authValidationService = new Mock<IAuthValidationService>();
            request = new GetAuthValidationsRequest();

            getAuthValidationsQuery = new GetAuthValidationsQuery(authValidationService.Object);
        }

        [Test]
        [TestCase(AuthValidationType.Username, true)]
        [TestCase(AuthValidationType.Username, false)]
        [TestCase(AuthValidationType.Email, true)]
        [TestCase(AuthValidationType.Email, false)]
        public async Task Handle_WhenCalled_ReturnGetAuthValidationsResponseWithIsAvailable(AuthValidationType type,
            bool exists)
        {
            authValidationService.Setup(a => a.UsernameExists(It.IsAny<string>()))
                .ReturnsAsync(exists);
            authValidationService.Setup(a => a.EmailExists(It.IsAny<string>()))
                .ReturnsAsync(exists);

            var result = await getAuthValidationsQuery.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<GetAuthValidationsResponse>());
            Assert.That(result.IsAvailable, Is.EqualTo(!exists));
        }
    }
}