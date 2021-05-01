using System.Threading.Tasks;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Services
{
    public interface IEmailSender
    {
        Task<bool> Send(EmailMessage emailMessage);
    }
}