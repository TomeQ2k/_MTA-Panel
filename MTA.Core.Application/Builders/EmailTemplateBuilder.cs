using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Builders
{
    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        private readonly EmailTemplate emailTemplate = new EmailTemplate();

        public IEmailTemplateBuilder SetName(string name)
        {
            emailTemplate.TemplateName = name;
            return this;
        }

        public IEmailTemplateBuilder SetSubject(string subject)
        {
            emailTemplate.Subject = subject;
            return this;
        }

        public IEmailTemplateBuilder SetContent(string content)
        {
            emailTemplate.Content = content;
            return this;
        }

        public IEmailTemplateBuilder WithParameters(string[] allowedParameters)
        {
            emailTemplate.AllowedParameters = allowedParameters;
            return this;
        }

        public EmailTemplate Build() => this.emailTemplate;
    }
}