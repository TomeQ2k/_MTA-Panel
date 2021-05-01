using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;

namespace MTA.Infrastructure.Shared.Services
{
    public class EmailTemplateGenerator : IEmailTemplateGenerator
    {
        private readonly IFilesManager filesManager;

        public EmailTemplateGenerator(IFilesManager filesManager)
        {
            this.filesManager = filesManager;
        }

        public async Task<EmailTemplate> FindEmailTemplate(string templateName)
        {
            var templateTuple = GetTemplateTuple(templateName);

            var templateFromFile = await filesManager.ReadFile($"{filesManager.WebRootPath}/{templateTuple.Path}");

            var emailTemplate =
                CreateEmailTemplateFromFile(templateFromFile, templateName, templateTuple.AllowedParameters);

            return emailTemplate;
        }

        public async Task<IEnumerable<EmailTemplate>> GetEmailTemplates()
        {
            var templatesPath = $"{filesManager.WebRootPath}/data/email_templates/";
            var templatesNames = filesManager.GetDirectoryFilesNames(templatesPath);

            var emailTemplates = new List<EmailTemplate>();

            foreach (var templateName in templatesNames)
                emailTemplates.Add(await FindEmailTemplate(templateName.Replace(templatesPath, string.Empty)));

            return emailTemplates;
        }

        public async Task EditEmailTemplate(string templateName, string subject, string content)
        {
            var templateTuple = GetTemplateTuple(templateName);

            var emailTemplate = new EmailTemplateBuilder()
                .SetSubject(subject)
                .SetContent(content)
                .Build();

            var templateFileToWrite =
                $"{emailTemplate.Subject}{EmailTemplateDictionary.TemplateSeparator}{emailTemplate.Content}";

            await filesManager.WriteFile(templateFileToWrite, $"{filesManager.WebRootPath}{templateTuple.Path}");
        }

        #region private

        private EmailTemplateTuple GetTemplateTuple(string templateName)
        {
            if (templateName.HasWhitespaces())
                throw new ArgumentNullException();

            var templateTuple = EmailTemplateDictionary.EmailTemplatesDictionary[templateName];

            return templateTuple;
        }

        private static EmailTemplate CreateEmailTemplateFromFile(string templateFromFile, string templateName,
            string[] allowedParameters)
        {
            if (string.IsNullOrEmpty(templateFromFile))
                throw new ArgumentException("Email template is empty");

            var templateSplittedIntoLines =
                templateFromFile.Split(EmailTemplateDictionary.TemplateSeparator);

            var emailTemplate = new EmailTemplateBuilder()
                .SetName(templateName)
                .SetSubject(templateSplittedIntoLines[0])
                .SetContent(templateSplittedIntoLines[1])
                .WithParameters(allowedParameters)
                .Build();

            return emailTemplate;
        }

        #endregion
    }
}