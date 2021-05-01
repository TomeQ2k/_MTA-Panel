using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyEmailTemplateGenerator
    {
        Task<EmailTemplate> FindEmailTemplate(string templateName);

        Task<IEnumerable<EmailTemplate>> GetEmailTemplates();
    }
}