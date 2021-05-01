using System.Collections.Generic;
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
    public class GenerateAnswersForPartTwoCommandTests
    {
        private Mock<IRPTestManager> rpTestManager;
        private GenerateAnswersForPartTwoCommand generateAnswersForPartTwoCommand;

        [SetUp]
        public void SetUp()
        {
            rpTestManager = new Mock<IRPTestManager>();
            rpTestManager.Setup(r =>
                    r.GenerateAnswersForPartTwo(It.IsAny<Dictionary<int,string>>()))
                .ReturnsAsync(true);

            generateAnswersForPartTwoCommand = new GenerateAnswersForPartTwoCommand(rpTestManager.Object);
        }
        
        [Test]
        public void Handle_ReviewFailed_ThrowServerException()
        {
            rpTestManager.Setup(r =>
                    r.GenerateAnswersForPartTwo(It.IsAny<Dictionary<int,string>>()))
                .ReturnsAsync(false);
            
            Assert.That(() => generateAnswersForPartTwoCommand.Handle(new GenerateAnswersForPartTwoRequest(), It.IsAny<CancellationToken>()), Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnGenerateAnswersForPartTwoResponse()
        {
            var result = await generateAnswersForPartTwoCommand.Handle(new GenerateAnswersForPartTwoRequest(), It.IsAny<CancellationToken>());
            
            Assert.That(result, Is.TypeOf<GenerateAnswersForPartTwoResponse>());
        }
    }
}