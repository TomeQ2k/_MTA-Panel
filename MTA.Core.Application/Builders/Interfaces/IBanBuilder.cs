using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IBanBuilder : IBuilder<Ban>
    {
        IBanBuilder BanAccount(int accountId);
        IBanBuilder BanSerial(string serial);
        IBanBuilder BanIp(string ip);
        IBanBuilder ByAdmin(int adminId);
        IBanBuilder SetReason(string reason);
    }
}