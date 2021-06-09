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
    public class RemoveNotificationCommandTests
    {
        private Mock<INotifier> notifier;
        private RemoveNotificationCommand removeNotificationCommand;

        [SetUp]
        public void SetUp()
        {
            notifier = new Mock<INotifier>();

            removeNotificationCommand = new RemoveNotificationCommand(notifier.Object);
        }

        [Test]
        public void Handle_RemoveNotificationFailed_ThrowCrudException()
        {
            notifier.Setup(n => n.RemoveNotification(It.IsAny<string>())).ReturnsAsync(false);

            Assert.That(() => removeNotificationCommand.Handle(new RemoveNotificationRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }
        [Test]
        public async Task Handle_WhenCalled_RemoveNotificationResponse()
        {
            notifier.Setup(n => n.RemoveNotification(It.IsAny<string>())).ReturnsAsync(true);

            var result = await removeNotificationCommand.Handle(new RemoveNotificationRequest(), It.IsAny<CancellationToken>());
            
            Assert.That(result, Is.TypeOf<RemoveNotificationResponse>().And.Not.Null);
        }
    }
}