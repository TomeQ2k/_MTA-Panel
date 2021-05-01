using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class MarkAsReadNotificationCommandTests
    {
        private Mock<INotifier> notifier;
        private MarkAsReadNotificationCommand markAsReadNotificationCommand;

        [SetUp]
        public void SetUp()
        {
            notifier = new Mock<INotifier>();

            markAsReadNotificationCommand = new MarkAsReadNotificationCommand(notifier.Object);
        }
        
        [Test]
        public void Handle_RemoveNotificationFailed_ThrowCrudException()
        {
            notifier.Setup(n => n.MarkAsRead(It.IsAny<string>())).ReturnsAsync(false);

            Assert.That(() => markAsReadNotificationCommand.Handle(new MarkAsReadNotificationRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }
        [Test]
        public async Task Handle_WhenCalled_RemoveNotificationResponse()
        {
            notifier.Setup(n => n.MarkAsRead(It.IsAny<string>())).ReturnsAsync(true);

            var result = await markAsReadNotificationCommand.Handle(new MarkAsReadNotificationRequest(), It.IsAny<CancellationToken>());
            
            Assert.That(result, Is.TypeOf<MarkAsReadNotificationResponse>().And.Not.Null);
        }
    }
}