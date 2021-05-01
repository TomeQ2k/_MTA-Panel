using MTA.Core.Application.Services;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class NotifierValidationService : INotifierValidationService
    {
        private readonly IHttpContextReader httpContextReader;

        public NotifierValidationService(IHttpContextReader httpContextReader)
        {
            this.httpContextReader = httpContextReader;
        }

        public bool ValidateUserPermissions(Notification notification)
            => httpContextReader.CurrentUserId == notification?.UserId;
    }
}