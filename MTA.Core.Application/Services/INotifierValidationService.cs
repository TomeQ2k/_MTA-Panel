using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface INotifierValidationService
    {
        bool ValidateUserPermissions(Notification notification);
    }
}