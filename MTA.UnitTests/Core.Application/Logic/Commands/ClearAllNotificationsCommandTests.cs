using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class ClearAllNotificationsCommandTests
    {
        private Mock<INotifier> notifier;
        private ClearAllNotificationsCommand clearAllNotificationsCommand;

        [SetUp]
        public void SetUp()
        {
            notifier = new Mock<INotifier>();

            clearAllNotificationsCommand = new ClearAllNotificationsCommand(notifier.Object);
        }

        [Test]
        public void Handle_ClearAllNotificationsFailed_ThrowCrudException()
        {
            notifier.Setup(n => n.ClearAllNotifications()).ReturnsAsync(false);

            Assert.That(
                () => clearAllNotificationsCommand.Handle(new ClearAllNotificationsRequest(),
                    It.IsAny<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_RemoveNotificationResponse()
        {
            notifier.Setup(n => n.ClearAllNotifications()).ReturnsAsync(true);

            var result =
                await clearAllNotificationsCommand.Handle(new ClearAllNotificationsRequest(),
                    It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<ClearAllNotificationsResponse>().And.Not.Null);
        }
    }
}