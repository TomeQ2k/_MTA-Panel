using System.Threading.Tasks;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Services
{
    public interface IDonationManager
    {
        Task<DonateServerResult> DonateServer(DonationType donationType, string code);
        Task<DonateServerResult> DonateServerDlcBrain(string code);
    }
}