using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Services
{
    public interface IEmailTemplateGenerator : IReadOnlyEmailTemplateGenerator
    {
        Task EditEmailTemplate(string templateName, string subject, string content);
    }
}