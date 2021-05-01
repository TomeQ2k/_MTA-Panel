using MTA.Core.Application.Models;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IEmailTemplateBuilder : IBuilder<EmailTemplate>
    {
        IEmailTemplateBuilder SetName(string name);
        IEmailTemplateBuilder SetSubject(string subject);
        IEmailTemplateBuilder SetContent(string content);
        IEmailTemplateBuilder WithParameters(string[] allowedParameters);
    }
}