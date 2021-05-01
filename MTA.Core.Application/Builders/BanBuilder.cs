using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class BanBuilder : IBanBuilder
    {
        private readonly Ban ban = new Ban();

        public IBanBuilder BanAccount(int accountId)
        {
            ban.SetAccountId(accountId);
            return this;
        }

        public IBanBuilder BanSerial(string serial)
        {
            ban.SetSerial(serial);
            return this;
        }

        public IBanBuilder BanIp(string ip)
        {
            ban.SetIp(ip);
            return this;
        }

        public IBanBuilder ByAdmin(int adminId)
        {
            ban.SetAdminId(adminId);
            return this;
        }

        public IBanBuilder SetReason(string reason)
        {
            ban.SetReason(reason);
            return this;
        }

        public Ban Build() => this.ban;
    }
}