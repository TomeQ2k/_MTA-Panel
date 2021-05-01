namespace MTA.Core.Application.Models
{
    public record EmailMessage
    (
        string Email,
        string Subject,
        string Message,
        string SenderEmail = null
    );
}