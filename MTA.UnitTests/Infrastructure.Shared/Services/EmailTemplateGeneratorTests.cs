using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Services;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class EmailTemplateGeneratorTests
    {
        private EmailTemplateGenerator emailTemplateGenerator;

        private Mock<IFilesManager> filesManager;

        [SetUp]
        public void SetUp()
        {
            filesManager = new Mock<IFilesManager>();

            emailTemplateGenerator = new EmailTemplateGenerator(filesManager.Object);
        }

        #region FindEmailTemplate

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void FindEmailTemplate_TemplateNameIsEmptyOrNull_ThrowArgumentNullException(string templateName)
        {
            Assert.That(() => emailTemplateGenerator.FindEmailTemplate(templateName), Throws.ArgumentNullException);
        }

        [Test]
        public void FindEmailTemplate_TemplateNotFound_ThrowKeyNotFoundException()
        {
            Assert.That(() => emailTemplateGenerator.FindEmailTemplate("notfound"),
                Throws.Exception.TypeOf<KeyNotFoundException>());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void FindEmailTemplate_TemplateFoundButIsEmptyOrNull_ThrowArgumentException(string emptyTemplateFile)
        {
            filesManager.Setup(fm => fm.ReadFile(It.IsAny<string>())).ReturnsAsync(emptyTemplateFile);

            Assert.That(() => emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate),
                Throws.ArgumentException);
        }

        [Test]
        public async Task FindEmailTemplate_RegisterTemplateFound_ReturnEmailTemplate()
        {
            filesManager.Setup(fm => fm.ReadFile(It.IsAny<string>()))
                .ReturnsAsync("SUBJECT$!$Test register body");

            var result = await emailTemplateGenerator.FindEmailTemplate(EmailTemplateDictionary.RegisterTemplate);

            Assert.That(result.TemplateName, Is.EqualTo(EmailTemplateDictionary.RegisterTemplate));
            Assert.That(result.Subject, Is.EqualTo("SUBJECT"));
            Assert.That(result.Content, Is.EqualTo("Test register body"));
        }

        #endregion

        #region GetEmailTemplates

        [Test]
        public async Task GetEmailTemplates_AnyTemplatesInDirectory_ReturnEmptyTemplates()
        {
            filesManager.Setup(fm => fm.GetDirectoryFilesNames(It.IsAny<string>()))
                .Returns(new List<string>());

            var result = await emailTemplateGenerator.GetEmailTemplates();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetEmailTemplates_RegisterTemplateFound_ReturnEmailTemplatesWithOneElement()
        {
            filesManager.Setup(fm => fm.ReadFile(It.IsAny<string>()))
                .ReturnsAsync("SUBJECT$!$Test register body");
            filesManager.Setup(fm => fm.GetDirectoryFilesNames(It.IsAny<string>()))
                .Returns(new List<string> {EmailTemplateDictionary.RegisterTemplate});

            var result = await emailTemplateGenerator.GetEmailTemplates();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Subject, Is.EqualTo("SUBJECT"));
            Assert.That(result.First().Content, Is.EqualTo("Test register body"));
        }

        #endregion

        #region EditEmailTemplate

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void EditEmailTemplate_TemplateNameIsEmptyOrNull_ThrowArgumentNullException(string templateName)
        {
            Assert.That(
                () => emailTemplateGenerator.EditEmailTemplate(templateName, It.IsAny<string>(), It.IsAny<string>()),
                Throws.ArgumentNullException);
        }

        [Test]
        public void EditEmailTemplate_TemplateNotFound_ThrowKeyNotFoundException()
        {
            Assert.That(
                () => emailTemplateGenerator.EditEmailTemplate("notfound", It.IsAny<string>(),
                    It.IsAny<string>()),
                Throws.Exception.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public async Task EditEmailTemplate_TemplateEdited_WriteFileShouldBeCalled()
        {
            await emailTemplateGenerator.EditEmailTemplate(EmailTemplateDictionary.RegisterTemplate, It.IsAny<string>(),
                It.IsAny<string>());

            filesManager.Verify(fm => fm.WriteFile(It.IsAny<string>(), It.IsAny<string>()));
        }

        #endregion
    }
}