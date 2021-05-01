using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class PassRPTestPartOneCommandTests
    {
        private Mock<IRPTestManager> rpTestManager;
        private PassRPTestPartOneCommand passRPTestPartOneCommand;

        [SetUp]
        public void SetUp()
        {
            var passRPTestPartOneResult = new PassRPTestPartOneResult();
            
            rpTestManager = new Mock<IRPTestManager>();
            rpTestManager.Setup(r =>
                    r.PassRPTestPartOne(It.IsAny<Dictionary<int,int>>()))
                .ReturnsAsync(passRPTestPartOneResult);

            passRPTestPartOneCommand = new PassRPTestPartOneCommand(rpTestManager.Object);
        }
        
        [Test]
        public void Handle_ReviewFailed_ThrowServerException()
        {
            rpTestManager.Setup(r =>
                    r.PassRPTestPartOne(It.IsAny<Dictionary<int,int>>()))
                .ReturnsAsync(() => null);
            
            Assert.That(() => passRPTestPartOneCommand.Handle(new PassRPTestPartOneRequest(), It.IsAny<CancellationToken>()), Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnPassRPTestPartOneResponse()
        {
            var result = await passRPTestPartOneCommand.Handle(new PassRPTestPartOneRequest(), It.IsAny<CancellationToken>());
            
            Assert.That(result, Is.TypeOf<PassRPTestPartOneResponse>());
        }
    }
}