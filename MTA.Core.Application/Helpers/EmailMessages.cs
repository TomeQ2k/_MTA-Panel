using MTA.Core.Application.Models;

namespace MTA.Core.Application.Helpers
{
    public static class EmailMessages
    {
        public static EmailMessage ActivationAccountEmail(string email, EmailTemplate emailTemplate)
            => new EmailMessage(
                Email: email,
                Subject: emailTemplate.Subject,
                Message: emailTemplate.Content
            );

        //TODO: Create template
        public static EmailMessage TransferCharacterEmail(string email) => new EmailMessage(
            email,
            "Transfer character",
            "Transfer character data succeeded");
    }
}