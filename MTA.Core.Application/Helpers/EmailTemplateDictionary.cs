using System.Collections.Generic;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Helpers
{
    public static class EmailTemplateDictionary
    {
        public const string TemplateSeparator = "$!$";

        public const string RegisterTemplate = "register_email.tpl";

        public static Dictionary<string, EmailTemplateTuple> EmailTemplatesDictionary =>
            new Dictionary<string, EmailTemplateTuple>
            {
                {
                    RegisterTemplate,
                    new($"/data/email_templates/{RegisterTemplate}", new[] {"{{username}}", "{{callbackUrl}}"})
                }
            };
    }
}