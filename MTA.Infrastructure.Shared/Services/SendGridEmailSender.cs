using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MTA.Infrastructure.Shared.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient emailClient;
        private readonly EmailSettings emailSettings;

        public SendGridEmailSender(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
            
            this.emailClient = new SendGridClient(this.emailSettings.ApiKey);
        }

        public async Task<bool> Send(EmailMessage emailMessage)
        {
            var emailContent =
                new EmailContent(
                    !string.IsNullOrEmpty(emailMessage.SenderEmail) ? emailMessage.SenderEmail : emailSettings.Sender,
                    emailMessage.Email);

            var email = MailHelper.CreateSingleEmail(emailContent.FromAddress, emailContent.ToAddress,
                emailMessage.Subject, emailMessage.Message, emailMessage.Message);

            var response = await emailClient.SendEmailAsync(email);

            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}