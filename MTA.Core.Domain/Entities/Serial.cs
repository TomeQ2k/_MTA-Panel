using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Serial : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("userid")] public int UserId { get; protected set; }
        [Column("serial")] public string Content { get; protected set; }
        [Column("creation_date")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("last_login_date")] public DateTime LastLoginDate { get; protected set; }
        [Column("last_login_ip")] public string LastLoginIp { get; protected set; }

        public static Serial Create(int userId, string content) =>
            new Serial {UserId = userId, Content = content};
    }
}