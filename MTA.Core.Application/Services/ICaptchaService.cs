using System.Threading.Tasks;

namespace MTA.Core.Application.Services
{
    public interface ICaptchaService
    {
        Task<bool> VerifyCaptcha(string captchaResponse);
    }
}