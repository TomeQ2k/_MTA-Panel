using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Ban : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("account")] public int AccountId { get; protected set; }
        [Column("serial")] public string Serial { get; protected set; }
        [Column("ip")] public string Ip { get; protected set; }
        [Column("admin")] public int AdminId { get; protected set; }
        [Column("reason")] public string Reason { get; protected set; }
        [Column("date")] public DateTime DateCreated { get; protected set; } = DateTime.Now;

        public void SetAccountId(int accountId) => AccountId = accountId;
        public void SetSerial(string serial) => Serial = serial;
        public void SetIp(string ip) => Ip = ip;
        public void SetAdminId(int adminId) => AdminId = adminId;
        public void SetReason(string reason) => Reason = reason;
    }
}