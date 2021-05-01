using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IRolesService
    {
        Task<bool> AdmitRole(User user, RoleType roleType);
        Task<bool> AdmitRole(int userId, RoleType roleType);

        Task<bool> RevokeRole(User user, RoleType roleType);
        Task<bool> RevokeRole(int userId, RoleType roleType);

        Task<bool> IsPermitted(User user, params RoleType[] roleTypes);
        Task<bool> IsPermitted(int userId, params RoleType[] roleTypes);
    }
}