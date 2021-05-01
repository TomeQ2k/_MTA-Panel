using System.Linq;

namespace MTA.Core.Application.Models
{
    public class EmailTemplate
    {
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public string[] AllowedParameters { get; set; }

        public EmailTemplate ReplaceParameters(params EmailTemplateParameter[] parameters)
        {
            if (parameters.Any() && parameters.All(p => AllowedParameters.Contains(p.Name)))
                parameters.ToList().ForEach(p => Content = Content.Replace(p.Name, p.Value));

            return this;
        }
    }
}