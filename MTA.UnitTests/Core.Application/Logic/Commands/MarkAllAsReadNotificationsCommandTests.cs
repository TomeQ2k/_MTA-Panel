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
    public class MarkAllAsReadNotificationsCommandTests
    {
        private Mock<INotifier> notifier;
        private MarkAllAsReadNotificationsCommand markAllAsReadNotificationsCommand;

        [SetUp]
        public void SetUp()
        {
            notifier = new Mock<INotifier>();

            markAllAsReadNotificationsCommand = new MarkAllAsReadNotificationsCommand(notifier.Object);
        }
        
        [Test]
        public void Handle_MarkAllAsReadFailed_ThrowCrudException()
        {
            notifier.Setup(n => n.MarkAllAsRead()).ReturnsAsync(false);

            Assert.That(() => markAllAsReadNotificationsCommand.Handle(new MarkAllAsReadNotificationsRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }
        [Test]
        public async Task Handle_WhenCalled_RemoveNotificationResponse()
        {
            notifier.Setup(n => n.MarkAllAsRead()).ReturnsAsync(true);

            var result = await markAllAsReadNotificationsCommand.Handle(new MarkAllAsReadNotificationsRequest(), It.IsAny<CancellationToken>());
            
            Assert.That(result, Is.TypeOf<MarkAllAsReadNotificationsResponse>().And.Not.Null);
        }
    }
}