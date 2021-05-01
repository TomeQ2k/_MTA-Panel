using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportSubscriberService
    {
        Task<ReportSubscriber> AddSubscriber(Report report, int userId);
        Task<bool> RemoveSubscriber(Report report, int userId);
    }
}