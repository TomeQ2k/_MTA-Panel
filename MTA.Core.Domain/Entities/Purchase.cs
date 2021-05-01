using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Purchase : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("name")] public string Name { get; protected set; }
        [Column("cost")] public int? Cost { get; protected set; }
        [Column("date")] public DateTime DatePurchased { get; protected set; } = DateTime.Now;
        [Column("account")] public int AccountId { get; protected set; }

        public User Account { get; protected set; }

        public static Purchase Create(string name, int accountId, int? cost = null) => new Purchase
        {
            Name = name,
            AccountId = accountId,
            Cost = cost
        };

        public void SetAccount(User user) => Account = user;
    }
}