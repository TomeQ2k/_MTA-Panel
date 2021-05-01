using SendGrid.Helpers.Mail;

namespace MTA.Core.Application.Models
{
    public record EmailContent
    {
        private readonly string sender;
        private readonly string receiver;

        public EmailAddress FromAddress { get; init; }
        public EmailAddress ToAddress { get; init; }

        public EmailContent(string sender, string receiver)
            => (this.sender, this.receiver, FromAddress, ToAddress)
                = (sender, receiver, new EmailAddress(sender), new EmailAddress(receiver));
    }
}