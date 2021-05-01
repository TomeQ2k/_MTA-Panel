using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IPurchaseService : IReadOnlyPurchaseService
    {
        Task<Purchase> CreatePurchase(string name, int userId, int? cost = null);
    }
}