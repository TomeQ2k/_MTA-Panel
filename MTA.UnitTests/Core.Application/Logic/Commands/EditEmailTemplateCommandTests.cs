using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class EditEmailTemplateCommandTests
    {
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;
        private EditEmailTemplateCommand editEmailTemplateCommand;

        [SetUp]
        public void Setup()
        {
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();

            editEmailTemplateCommand = new EditEmailTemplateCommand(emailTemplateGenerator.Object);
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnEditEmailTemplateResponse()
        {
            var request = new EditEmailTemplateRequest
            {
                TemplateName = "test",
                Content = "test",
                Subject = "test"
            };

            emailTemplateGenerator.Setup(etg =>
                etg.EditEmailTemplate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var result = await editEmailTemplateCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<EditEmailTemplateResponse>());
        }
    }
}