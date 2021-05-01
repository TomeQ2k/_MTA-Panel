using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IAdminActionService : IReadOnlyAdminActionService
    {
        Task<AdminAction> InsertAdminAction(AdminAction adminAction);
    }
}