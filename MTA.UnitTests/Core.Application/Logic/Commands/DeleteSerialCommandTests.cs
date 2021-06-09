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
    public class DeleteSerialCommandTests
    {
        private Mock<ISerialService> serialService;
        private DeleteSerialCommand deleteSerialCommand;
        private DeleteSerialRequest deleteSerialRequest;

        [SetUp]
        public void SetUp()
        {
            deleteSerialRequest = new DeleteSerialRequest {SerialId = 1};
            serialService = new Mock<ISerialService>();

            deleteSerialCommand = new DeleteSerialCommand(serialService.Object);
        }

        [Test]
        public void Handle_DeletingSerialFailed_ThrowDatabaseException()
        {
            serialService.Setup(ss => ss.DeleteSerial(It.IsAny<int>()))
                .ReturnsAsync(false);

            Assert.That(
                () => deleteSerialCommand.Handle(deleteSerialRequest, It.IsAny<CancellationToken>()),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task Handle_WhenCalled_RetrunDeleteSerialResponse()
        {
            serialService.Setup(ss => ss.DeleteSerial(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await deleteSerialCommand.Handle(deleteSerialRequest, It.IsAny<CancellationToken>());
            
            Assert.That(result, Is.TypeOf<DeleteSerialResponse>().And.Not.Null);
        }
    }
}